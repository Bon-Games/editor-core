using UnityEditor;

namespace BonGames.EasyBuilder
{
    public class AndroidBuilder : ProjectBuilder
    {
        public AndroidBuilder(EEnvironment environment) : base(EAppTarget.Client, BuildTarget.Android, environment)
        {
            
        }

        protected override void SetupInternally()
        {
            base.SetupInternally();
            /// EditorUserBuildSettings
            bool isReleaseBuild = Environment == EEnvironment.Release || Environment == EEnvironment.Distribution;
            EditorUserBuildSettings.development = Environment == EEnvironment.Debug || Environment == EEnvironment.Development;
            EditorUserBuildSettings.connectProfiler = Environment == EEnvironment.Debug;
            EditorUserBuildSettings.buildWithDeepProfilingSupport = Environment == EEnvironment.Debug;
            EditorUserBuildSettings.allowDebugging = Environment == EEnvironment.Debug;
            EditorUserBuildSettings.buildAppBundle = Environment == EEnvironment.Distribution;
            EditorUserBuildSettings.androidBuildType = GetBuildType();
            EditorUserBuildSettings.androidCreateSymbols = isReleaseBuild ? AndroidCreateSymbols.Public : AndroidCreateSymbols.Disabled;
            /// -- EditorUserBuildSettings
            

        }

        protected override void SignApp()
        {
            base.SignApp();
            string ksPath = BuildArguments.Android.GetKeystorePath();
            bool useCustomKey = !string.IsNullOrEmpty(ksPath);
            if (useCustomKey)
            {
                PlayerSettings.Android.useCustomKeystore = useCustomKey;
                PlayerSettings.Android.keystoreName = ksPath; 
                PlayerSettings.Android.keystorePass = BuildArguments.Android.GetKeystorePassword();
                PlayerSettings.Android.keyaliasName = BuildArguments.Android.GetAlias();
                PlayerSettings.Android.keyaliasPass = BuildArguments.Android.GetAliasPassword();
            }            
        }

        private AndroidBuildType GetBuildType()
        {
            switch (Environment)
            {
                case EEnvironment.Debug:
                    return AndroidBuildType.Debug;
                case EEnvironment.Development:                    
                case EEnvironment.Staging:
                    return AndroidBuildType.Development;                    
                case EEnvironment.Release:
                case EEnvironment.Distribution:
                    return AndroidBuildType.Release;
            }
            return AndroidBuildType.Development;
        }
    }
}
