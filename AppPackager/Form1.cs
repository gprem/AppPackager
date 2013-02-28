using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace AppPackager
{
    public partial class Form1 : Form
    {
        private string sourcePath = "";
        private string targetVersionStr = "";
        private string sourceDllFolder = "";
        private List<string> localizationFolderList = new List<string>();
        private Regex versionRegex = new Regex(@"\d\.\d+\.\d+\.\d+");
        private string htmlFullFileName = "";
        private string backupFolderPath = "";
        private string newAppFilesFolderPath = "";
        private string mageUIPath = "";
        private string certificatePath = "";
        private Version curAppVersion, targetVersion;

        private ApplicationManifest applicationManifest = null;
        private DeployManifest deploymentManifest = null;
        private BindingList<ApplicationFile> m_Files = new BindingList<ApplicationFile>();
        private List<AssemblyReference> m_Prequisites = new List<AssemblyReference>();
        private List<string> exceptionFileList = new List<string>(new string[] { "AssemblyPlanner.exe.config.deploy", "AssemblyPlanner.exe.manifest", "Proplanner.ico.deploy", "Proplanner.license.deploy" });

        
        public Form1()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.AppPackager;

            this.Text += string.Format(" ({0})", Application.ProductVersion);

            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            this.folderBrowserDialog1.SelectedPath = ConfigurationManager.AppSettings["startupFolder"];

            this.UpdateSourceDllFolder(ConfigurationManager.AppSettings["sourceDllFolder"]);

            var localizationFolderStr = ConfigurationManager.AppSettings["localizationFolderList"];
            foreach (var folderName in localizationFolderStr.Split(','))
            {
                localizationFolderList.Add(folderName.Trim());
            }      
            
            this.mageUIPath = Path.Combine(ConfigurationManager.AppSettings["magePath"], "mageui.exe");

            this.certificatePath = ConfigurationManager.AppSettings["certificatePath"];

            this.tbTargetVersion.Text = Properties.Settings.Default.DefaultTargetVersion;
                        
#if DEBUG
            this.tbSourcePath.Text = @"C:\Temp\ClickonceTesting\BEN-ASY";
#endif
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            this.tbSourcePath.Text = this.folderBrowserDialog1.SelectedPath;

            this.RefreshSelection();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //TODO: Check if all required config file settings values are valid.

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.Reset();

                this.CollectCurrentInfo();

                this.PerformBackup();

                //TODO: Temp code. Rmeove after UpdateApplicationManifest has been completed
                this.UpdateManifestVersions();

                this.RefreshDlls();

                //Change application manifest
                //this.UpdateApplicationManifest();

                //Change deployment manifest files
                //this.UpdateDeploymentManifest();

                //Change HTML file version
                ChangeHTMLVersionInfo(this.htmlFullFileName, this.tbTargetVersion.Text.Trim());

                //Update the UI
                this.RefreshSelection();

                //Show success message
                UIHelper.ShowInformation("Update was completed successfully");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(string.Concat("An error occured while trying to perform the update: ", ex.Message));

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (chkRollbackOnError.Checked)
                    {
                        //Perform Rollback
                        this.PerformRollback();

                        UIHelper.ShowInformation("Update changes were rolledback successfully");
                    }
                }
                catch (Exception exRollback)
                {
                    UIHelper.ShowError(string.Concat("An error occured while trying to perform the update: ", exRollback.Message));
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshSelection()
        {
            this.Reset();

            this.CollectCurrentInfo();

            this.tbTargetVersion_TextChanged(this, EventArgs.Empty);
        }

        private void PerformRollback()
        {
            //Delete the modified files
            string appFilesFolderPath = Path.GetDirectoryName(this.applicationManifest.SourcePath);
            if (Directory.Exists(appFilesFolderPath))
                Directory.Delete(appFilesFolderPath, true);

            if (Directory.Exists(this.newAppFilesFolderPath))
                Directory.Delete(newAppFilesFolderPath, true);  

            //Restore the files from the backup
            if (!Directory.Exists(this.backupFolderPath))
            {
                throw new Exception("Backup folder does not exists to perform a rollback.");
            }

            CopyFolderRecursively(this.backupFolderPath, this.sourcePath);
        }

        private void UpdateSourceDllFolder(string newUpdateDllFolder)
        {
            this.sourceDllFolder = newUpdateDllFolder.Trim();
            this.tbUpdateDllFolder.Text = this.sourceDllFolder;

            //Determine the target version automatically
            bool isValid = false;
            if (Directory.Exists(this.sourceDllFolder))
            {
                string[] files = Directory.GetFiles(this.sourceDllFolder, "AssemblyPlanner.exe");
                if (files.Length > 0)
                {
                    isValid = true;
                    this.tbTargetVersion.TextChanged -=tbTargetVersion_TextChanged;

                    this.tbTargetVersion.Text = System.Reflection.AssemblyName.GetAssemblyName(files[0]).Version.ToString();

                    this.tbTargetVersion.TextChanged += tbTargetVersion_TextChanged;
                }
            }

            if (!isValid)
                this.errorProvider1.SetError(this.tbUpdateDllFolder, "Selected folder does not contain required files.");
        }

        private void Reset()
        {
            //Clear all errors
            this.errorProvider1.Clear();

            this.tbCurrentAppVersion.Clear();
            this.tbCurrentHTMLVersion.Clear();
            this.tbCurrentDepolymentVersion.Clear();

            applicationManifest = null;
            deploymentManifest = null;

            this.targetVersionStr = "";
            this.sourceDllFolder = "";
            this.backupFolderPath = "";
            this.htmlFullFileName = "";
            this.newAppFilesFolderPath = "";
        }

        private void CollectCurrentInfo()
        {
            this.sourcePath = this.tbSourcePath.Text.Trim();
            this.sourceDllFolder = this.tbUpdateDllFolder.Text.Trim();
            this.targetVersionStr = this.tbTargetVersion.Text.Trim();
            
            //Check if the source path is valid
            if (String.IsNullOrEmpty(this.sourcePath))
            {
                this.errorProvider1.SetError(this.tbSourcePath, "Select valid clickonce package folder to update");
                return;
            }

            //TODO: Check the updateDllFolder path validity
            
            this.tbCurrentAppVersion.Clear();
            this.tbCurrentHTMLVersion.Clear();
            this.tbCurrentDepolymentVersion.Clear();

            string deploymentManifestFileName = "";

            DirectoryInfo dInfo = new DirectoryInfo(sourcePath);
            FileInfo[] appMainfestfiles = dInfo.GetFiles("*.application");
            foreach (var appMainfestfile in appMainfestfiles)
            {
                //Search for the manifest with a filename of AssemblyPlanner_XXX.application or AssemblyPlanner.application
                //We need to ignore files with a name like AssemblyPlanner_XXX_5_4_1_0.application
                string fileName = Path.GetFileNameWithoutExtension(appMainfestfile.Name);
                string[] splits = fileName.Split('_');
                if (splits.Length >= 4)
                    continue;

                deploymentManifestFileName = appMainfestfile.FullName;
            }

            this.htmlFullFileName = Path.Combine(sourcePath, "default.htm");

            if ((deploymentManifestFileName.Length == 0) || !File.Exists(deploymentManifestFileName))
            {
                MessageBox.Show(deploymentManifestFileName);
                throw new Exception(string.Concat("Could not locate the deployment manifest - ", deploymentManifestFileName));
            }

            ManifestHelper.LoadDeploymentManifestData(deploymentManifestFileName, out deploymentManifest, out applicationManifest);

            m_Prequisites = ManifestHelper.GetPrerequisites(applicationManifest);
            m_Files = ManifestHelper.GetFiles(applicationManifest);

            this.tbCurrentDepolymentVersion.Text = deploymentManifest.AssemblyIdentity.Version;
            this.tbCurrentAppVersion.Text = applicationManifest.AssemblyIdentity.Version;
            this.tbCurrentHTMLVersion.Text = this.ExtractVersionInfoFromHTML(htmlFullFileName);

            //Validate the version numbers
            if (string.Compare(tbCurrentAppVersion.Text, tbCurrentDepolymentVersion.Text) != 0)
                this.errorProvider1.SetError(this.tbCurrentAppVersion, "Different from application version");
            if (string.Compare(tbCurrentAppVersion.Text, tbCurrentHTMLVersion.Text) != 0)
                this.errorProvider1.SetError(this.tbCurrentHTMLVersion, "Different from application version");

            //If the target version is empty set it to the current application version
            if (String.IsNullOrEmpty(targetVersionStr))
            {
                this.tbTargetVersion.Text = this.tbCurrentAppVersion.Text;
                this.targetVersionStr = this.tbTargetVersion.Text;
            }

            //Update the local variables
            this.curAppVersion = new Version(this.tbCurrentAppVersion.Text);
            this.targetVersion = new Version(this.tbTargetVersion.Text.Trim());

            //TODO: Show we warn about the same target version?
        }

        private string ExtractVersionInfoFromHTML(string filename)
        {
            string fileContentStr = File.ReadAllText(filename);
            string versionInfo = "";
            
            Match match = versionRegex.Match(fileContentStr);
            if (match.Success)
            {
                versionInfo = match.Value;
            }

            return versionInfo;

        }

        private void ChangeHTMLVersionInfo(string fileName, string targetVersionStr)
        {
            string fileContentStr = File.ReadAllText(fileName);
            string newHTMLContentStr = versionRegex.Replace(fileContentStr, targetVersionStr);

            File.WriteAllText(fileName, newHTMLContentStr);
        }

        private void PerformBackup()
        {
            this.backupFolderPath = Path.Combine(this.sourcePath, "Backup");
            if (Directory.Exists(backupFolderPath))
            {
                Directory.Delete(backupFolderPath, true);
            }

            Directory.CreateDirectory(backupFolderPath);
            while (!Directory.Exists(backupFolderPath))
                Application.DoEvents();
         
            //Copy all the files
            foreach (string fileName in Directory.GetFiles(this.sourcePath))
            {
                if (Path.GetExtension(fileName).ToLower().Equals(".pfx") ||
                    Path.GetExtension(fileName).ToLower().Equals(".exe"))
                    continue;
                File.Copy(fileName, Path.Combine(backupFolderPath, Path.GetFileName(fileName)));
            }

            //Copy all the directory
            foreach (string dirName in Directory.GetDirectories(this.sourcePath, "AssemblyPlanner_*"))
            {
                CopyFolderRecursively(dirName, Path.Combine(backupFolderPath, Path.GetFileName(dirName)));
            }
        }

        private static void CopyFolderRecursively( string sourceFolder, string destFolder )
        {
            if (!Directory.Exists( destFolder ))
                Directory.CreateDirectory( destFolder );

            while (!Directory.Exists(destFolder))
            {
                Application.DoEvents();
            }

            string[] files = Directory.GetFiles( sourceFolder );
            foreach (string file in files)
            {
                string name = Path.GetFileName( file );
                string dest = Path.Combine( destFolder, name );
                File.Copy( file, dest, true );
            }
            string[] folders = Directory.GetDirectories( sourceFolder );
            foreach (string folder in folders)
            {
                string name = Path.GetFileName( folder );
                string dest = Path.Combine( destFolder, name );
                CopyFolderRecursively(folder, dest);
            }
        }

        private void RefreshDlls()
        {            
            string oldAppFolderName = Path.GetDirectoryName(deploymentManifest.EntryPoint.TargetPath);
            string newAppFolderName = string.Format("AssemblyPlanner_{0}_{1}_{2}_{3}", targetVersion.Major, targetVersion.Minor, targetVersion.Build, targetVersion.Revision);
            this.newAppFilesFolderPath = Path.Combine(this.sourcePath, newAppFolderName);

            //Create the new Application files folder name
            if (!oldAppFolderName.Equals(newAppFolderName))
            {
                Directory.CreateDirectory(newAppFilesFolderPath);
                while (!Directory.Exists(newAppFilesFolderPath))
                    Application.DoEvents();

                CopyFolderRecursively(Path.Combine(this.sourcePath, oldAppFolderName), newAppFilesFolderPath);

                Directory.Delete(Path.Combine(this.sourcePath, oldAppFolderName), true);
            }

            DirectoryInfo dInfo = new DirectoryInfo(newAppFilesFolderPath);

            //Delete the old dlls
            foreach (FileInfo delFileInfo in dInfo.GetFiles())
            {
                if(exceptionFileList.Contains(delFileInfo.Name))
                    continue;

                delFileInfo.Delete();
            }
            foreach (DirectoryInfo delDirInfo in dInfo.GetDirectories())
            {
                delDirInfo.Delete(true);
            }

            //Copy the new dlls
            foreach (string newDllFullName in Directory.GetFiles(this.sourceDllFolder, "*.dll"))
            {
                File.Copy(newDllFullName, Path.Combine(newAppFilesFolderPath, Path.GetFileName(newDllFullName)));
            }

            //Copy the exe
            File.Copy(Path.Combine(this.sourceDllFolder, "AssemblyPlanner.exe"), Path.Combine(newAppFilesFolderPath, "AssemblyPlanner.exe"));

            //Copy the localization folders
            var localicationFullFolderName = "";
            var targetLocationFullFolderName = "";
            foreach (var localicationFolderName in this.localizationFolderList)
            {
                localicationFullFolderName = Path.Combine(this.sourceDllFolder, localicationFolderName);
                targetLocationFullFolderName = Path.Combine(newAppFilesFolderPath, localicationFolderName);
                if (Directory.Exists(localicationFullFolderName))
                {
                    CopyFolderRecursively(localicationFullFolderName, targetLocationFullFolderName);
                }
            }

            //Create the new version depoyment manifest
            string oldVerDepManifestFilename = string.Format("{0}_{1}_{2}_{3}_{4}", Path.GetFileNameWithoutExtension(deploymentManifest.SourcePath), curAppVersion.Major, curAppVersion.Minor, curAppVersion.Build, curAppVersion.Revision);
            oldVerDepManifestFilename = string.Concat(oldVerDepManifestFilename, ".application");
            oldVerDepManifestFilename = Path.Combine(Path.GetDirectoryName(deploymentManifest.SourcePath), oldVerDepManifestFilename);
            
            string newVerDepManifestFilename = string.Format("{0}_{1}_{2}_{3}_{4}", Path.GetFileNameWithoutExtension(deploymentManifest.SourcePath), targetVersion.Major, targetVersion.Minor, targetVersion.Build, targetVersion.Revision);
            newVerDepManifestFilename = string.Concat(newVerDepManifestFilename, ".application");
            newVerDepManifestFilename = Path.Combine(Path.GetDirectoryName(deploymentManifest.SourcePath), newVerDepManifestFilename);

            if (!File.Exists(oldVerDepManifestFilename))
                throw new Exception(string.Concat("Could not locate the version specific deployment manifest file: ", oldVerDepManifestFilename));

            File.Copy(oldVerDepManifestFilename, newVerDepManifestFilename);
            File.Delete(oldVerDepManifestFilename);

        }

        private void UpdateApplicationManifest()
        {
            if (this.applicationManifest == null)
                throw new Exception("Applciation manifest has not yet been loaded.");

            string[] files = Directory.GetFiles(this.newAppFilesFolderPath,"*.*", SearchOption.AllDirectories);

            List<string> selFileList = new List<string>();
            foreach (var file in files)
            {
                if (!this.exceptionFileList.Contains(Path.GetFileName(file)))
                    selFileList.Add(file);
            }

            //Remove the unwanted entries from the exiswting appFiles list            
            List<AssemblyReference> delList = new List<AssemblyReference>();
            List<string> assyRefExceptionList = new List<string>(new string[] { "Microsoft.Windows.CommonLanguageRuntime, Version=2.0.50727.0", "AssemblyPlanner.exe" });
            foreach (AssemblyReference assyRef in this.applicationManifest.AssemblyReferences)
            {
                if (!assyRefExceptionList.Contains(assyRef.ToString()))
                    delList.Add(assyRef);
            }
            foreach (var assyRef in delList)
            {
                this.applicationManifest.AssemblyReferences.Remove(assyRef);
            }

            //Refresh the applicationFileList local variable
            this.m_Files = ManifestHelper.GetFiles(this.applicationManifest);
                                    
            ManifestHelper.AddFilesInPlace(selFileList.ToArray(), true, this.m_Files, this.applicationManifest);

            //Save the manifest file refrence values
            ManifestHelper.SaveManifestValues(this.applicationManifest.AssemblyIdentity.Name, this.targetVersionStr, deploymentManifest.DeploymentUrl, deploymentManifest, applicationManifest, this.m_Files, this.m_Prequisites);

            // Update hashes and size info for files
            //this.applicationManifest.ResolveFiles();
            this.applicationManifest.UpdateFileInfo();

            //Set the application icon
            this.applicationManifest.IconFile = "Proplanner.ico";
            this.applicationManifest.ConfigFile = "AssemblyPlanner.exe.config";
            
            //Perform validation
            this.applicationManifest.Validate();
            if (this.applicationManifest.OutputMessages.ErrorCount > 0)
            {
                using (ErrorForm eForm = new ErrorForm())
                {
                    eForm.ErrorMessage = "Error: Application manifest validation failed.";
                    List<string> errorDetails = new List<string>();
                    foreach (OutputMessage errMsg in this.applicationManifest.OutputMessages)
                    {
                        errorDetails.Add(errMsg.Text);
                    }

                    DialogResult res = eForm.ShowDialog(this);
                    if(res != System.Windows.Forms.DialogResult.OK)
                        throw new Exception("Application manifest validation failed.");
                }                
            }

            // Write app manifest 
            ManifestWriter.WriteManifest(this.applicationManifest);

            // Sign app manifest 
            ManifestHelper.SignManifest(this.applicationManifest, this.certificatePath, "");           
        }

        private void UpdateDeploymentManifest()
        {
            if (this.deploymentManifest == null)
                throw new Exception("Deployment manifest has not yet been loaded.");

            ManifestHelper.UpdateDeployManifestAppReference(this.deploymentManifest, this.applicationManifest);

            this.deploymentManifest.MinimumRequiredVersion = this.targetVersionStr;

            this.deploymentManifest.Validate();

            if (this.deploymentManifest.OutputMessages.ErrorCount > 0)
            {
                throw new Exception("Deployment manifest validation failed.");
            }
            
            // Write deploy manifest 
            ManifestWriter.WriteManifest(this.deploymentManifest);
            
            // sign deploy manifest 
            ManifestHelper.SignManifest(this.deploymentManifest, this.certificatePath, "");
        }

        private void UpdateManifestVersions()
        {
            //Update application manifest
            if (this.applicationManifest == null)
                throw new Exception("Applciation manifest has not yet been loaded.");

            string fileContents = File.ReadAllText(this.applicationManifest.SourcePath);
            string oldVerString = string.Format("version=\"{0}\"", applicationManifest.AssemblyIdentity.Version);
            string newVerString = string.Format("version=\"{0}\"", tbTargetVersion.Text.Trim());
            fileContents = fileContents.Replace(oldVerString, newVerString);

            File.WriteAllText(this.applicationManifest.SourcePath, fileContents);

            //Update deployment manifest
            if(this.deploymentManifest == null)
                throw new Exception("Deployment manifest has not yet been loaded.");
            
            string fileContents2 = File.ReadAllText(this.deploymentManifest.SourcePath);
            oldVerString = string.Format("version=\"{0}\"", deploymentManifest.AssemblyIdentity.Version);
            newVerString = string.Format("version=\"{0}\"", tbTargetVersion.Text.Trim());
            fileContents2 = fileContents2.Replace(oldVerString, newVerString);

            string oldMinVerString = string.Format("minimumRequiredVersion=\"{0}\"", deploymentManifest.AssemblyIdentity.Version);
            string newMinVerString = string.Format("minimumRequiredVersion=\"{0}\"", tbTargetVersion.Text.Trim());
            fileContents2 = fileContents2.Replace(oldMinVerString, newMinVerString);

            string oldRefPath = string.Format("codebase=\"AssemblyPlanner_{0}_{1}_{2}_{3}\\AssemblyPlanner.exe.manifest\"", 
                curAppVersion.Major, curAppVersion.Minor, curAppVersion.Build, curAppVersion.Revision);
            string newRefPath = string.Format("codebase=\"AssemblyPlanner_{0}_{1}_{2}_{3}\\AssemblyPlanner.exe.manifest\"", 
                targetVersion.Major, targetVersion.Minor, targetVersion.Build, targetVersion.Revision);
            fileContents2 = fileContents2.Replace(oldRefPath, newRefPath);
            
            File.WriteAllText(this.deploymentManifest.SourcePath, fileContents2);
        }

        private void tbTargetVersion_TextChanged(object sender, EventArgs e)
        {
            this.errorProvider1.SetError(this.tbTargetVersion, "");

            if (applicationManifest != null)
            {
                bool isValid = false;
                if (versionRegex.IsMatch(this.tbTargetVersion.Text.Trim()))
                {
                    Version targetVer = new Version(this.tbTargetVersion.Text.Trim());
                    if (targetVer.CompareTo(new Version(applicationManifest.AssemblyIdentity.Version)) >= 0)
                        isValid = true;

                    Properties.Settings.Default.DefaultTargetVersion = targetVer.ToString();
                    Properties.Settings.Default.Save();
                }

                if (!isValid)
                    this.errorProvider1.SetError(this.tbTargetVersion, "Invalid Product Version");
            }
        }

        private void btnCheckConfig_Click(object sender, EventArgs e)
        {
            string stdConfigFilePath = Path.Combine(this.sourceDllFolder, "AssemblyPlanner.exe.config");
            string appConfigFilePath = Path.Combine(Path.GetDirectoryName(this.applicationManifest.SourcePath), "AssemblyPlanner.exe.config.deploy");

            string examDiffProCommand = @"C:\Program Files\ExamDiff Pro\ExamDiff.exe";
            string cmdArgs = string.Format("{0} {1}", appConfigFilePath, stdConfigFilePath);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(examDiffProCommand, cmdArgs));
        }

        private void btnEditAppManifest_Click(object sender, EventArgs e)
        {
            if (!File.Exists(mageUIPath))
            {
                UIHelper.ShowError("Could not find mageui.exe. Please check the magePath appsetting in the config file.");
                return;
            }

            if ((this.applicationManifest == null) || (!File.Exists(this.applicationManifest.SourcePath)))
            {
                UIHelper.ShowError("Invalid application manifest information.");
                return;
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(this.mageUIPath, this.applicationManifest.SourcePath));
        }

        private void btnEditDepManifest_Click(object sender, EventArgs e)
        {
            if (!File.Exists(mageUIPath))
            {
                UIHelper.ShowError("Could not find mageui.exe. Please check the magePath appsetting in the config file.");
                return;
            }

            if ((this.applicationManifest == null) || (!File.Exists(this.deploymentManifest.SourcePath)))
            {
                UIHelper.ShowError("Invalid deployment manifest information.");
                return;
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(this.mageUIPath, this.deploymentManifest.SourcePath));
        }

        private void btnDeploy_Click(object sender, EventArgs e)
        {
            string deploymentFolderPath = this.tbDeploymentFolderPath.Text.Trim();
            if(deploymentFolderPath.Length == 0)
            {
               UIHelper.ShowInformation("Please select a valid Deployment folder and try again");
               return;
            }

            if (!Directory.Exists(deploymentFolderPath))
            {
                UIHelper.ShowError("The deployment folder path cannot be found or accessed. Please check path and try again.");
                return;
            }

            //Copy the html file.
            File.Copy(this.htmlFullFileName, Path.Combine(deploymentFolderPath, Path.GetFileName(htmlFullFileName)));

            //Copy the deployment files.

            
            //Copy the dll folder recursively

        }
    }
}
