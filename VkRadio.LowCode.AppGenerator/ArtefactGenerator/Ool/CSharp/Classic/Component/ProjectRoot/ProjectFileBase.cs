using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot
{
    public class ProjectFileBase: ProjectFile
    {
        public ProjectFileBase(CSharpProjectBase in_projectPackage)
            : base(in_projectPackage)
        {
            _lastLineWNewLine = false;

            var generator = in_projectPackage.ParentPackage.ArtefactGenerationTarget.Generator;
            var cSharpAppTarget = generator.Target.Parent;

            _predefinedCode.Add($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            _predefinedCode.Add($"<Project ToolsVersion=\"12.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
            _predefinedCode.Add($"  <PropertyGroup>");
            _predefinedCode.Add($"    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>");
            _predefinedCode.Add($"    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
            _predefinedCode.Add($"    <ProductVersion>8.0.30703</ProductVersion>");
            _predefinedCode.Add($"    <SchemaVersion>2.0</SchemaVersion>");
            _predefinedCode.Add($"    <ProjectGuid>{{{in_projectPackage.ProjectGuid.ToString().ToUpper()}}}</ProjectGuid>");
            _predefinedCode.Add($"    <OutputType>Library</OutputType>");
            _predefinedCode.Add($"    <AppDesignerFolder>Properties</AppDesignerFolder>");
            _predefinedCode.Add($"    <RootNamespace>{in_projectPackage.RootNamespace}</RootNamespace>");
            _predefinedCode.Add($"    <AssemblyName>{in_projectPackage.RootNamespace}</AssemblyName>");
            _predefinedCode.Add($"    <TargetFrameworkVersion>v{generator.Target.Parent.DotNetFramework}</TargetFrameworkVersion>");
            _predefinedCode.Add($"    <FileAlignment>512</FileAlignment>");
            _predefinedCode.Add($"    <TargetFrameworkProfile />");
            _predefinedCode.Add($"  </PropertyGroup>");
            _predefinedCode.Add($"  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
            _predefinedCode.Add($"    <DebugSymbols>true</DebugSymbols>");
            _predefinedCode.Add($"    <DebugType>full</DebugType>");
            _predefinedCode.Add($"    <Optimize>false</Optimize>");
            _predefinedCode.Add($"    <OutputPath>bin\\Debug\\</OutputPath>");
            _predefinedCode.Add($"    <DefineConstants>DEBUG;TRACE</DefineConstants>");
            _predefinedCode.Add($"    <ErrorReport>prompt</ErrorReport>");
            _predefinedCode.Add($"    <WarningLevel>4</WarningLevel>");
            _predefinedCode.Add($"    <Prefer32Bit>false</Prefer32Bit>");
            _predefinedCode.Add($"  </PropertyGroup>");
            _predefinedCode.Add($"  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
            _predefinedCode.Add($"    <DebugType>pdbonly</DebugType>");
            _predefinedCode.Add($"    <Optimize>true</Optimize>");
            _predefinedCode.Add($"    <OutputPath>bin\\Release\\</OutputPath>");
            _predefinedCode.Add($"    <DefineConstants>TRACE</DefineConstants>");
            _predefinedCode.Add($"    <ErrorReport>prompt</ErrorReport>");
            _predefinedCode.Add($"    <WarningLevel>4</WarningLevel>");
            _predefinedCode.Add($"    <Prefer32Bit>false</Prefer32Bit>");
            _predefinedCode.Add($"  </PropertyGroup>");
            _predefinedCode.Add($"  <ItemGroup>");
            _predefinedCode.Add($"    <Reference Include=\"System\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Core\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Xml.Linq\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Data.DataSetExtensions\" />");
            _predefinedCode.Add($"    <Reference Include=\"Microsoft.CSharp\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Data\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Deployment\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Drawing\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Windows.Forms\" />");
            _predefinedCode.Add($"    <Reference Include=\"System.Xml\" />");
            _predefinedCode.Add($"  </ItemGroup>");
            _predefinedCode.Add($"  <ItemGroup>");

            #region Including components for each data object type.
            var guiElPackage = Package.GuiPackage.ElementsPackage;
            foreach (var component in guiElPackage.MainComponents.Values)
            {
                var baseClassName = component.MainClass.Name.Substring(3);
                var designerComponentName = $"DOP{baseClassName}.Designer.cs";

                // Element cards (domain model object editors).
                _predefinedCode.Add($"    <Compile Include=\"Gui\\Elements\\{component.Name}\">");
                _predefinedCode.Add($"      <SubType>UserControl</SubType>");
                _predefinedCode.Add($"    </Compile>");
                _predefinedCode.Add($"    <Compile Include=\"Gui\\Elements\\{designerComponentName}\">");
                _predefinedCode.Add($"      <DependentUpon>{component.Name}</DependentUpon>");
                _predefinedCode.Add($"    </Compile>");

                // Element lists (tables for view and manipulate domain model objects).
                var listClassName = $"DOL{baseClassName}";
                _predefinedCode.Add($"    <Compile Include=\"Gui\\Lists\\{listClassName}.cs\">");
                _predefinedCode.Add($"      <SubType>UserControl</SubType>");
                _predefinedCode.Add($"    </Compile>");
                _predefinedCode.Add($"    <Compile Include=\"Gui\\Lists\\{listClassName}.Designer.cs\">");
                _predefinedCode.Add($"      <DependentUpon>{listClassName}.cs</DependentUpon>");
                _predefinedCode.Add($"    </Compile>");

                // UI launchers.
                _predefinedCode.Add($"    <Compile Include=\"Gui\\Launchers\\Uil{baseClassName}.cs\" />");

                // Components of domain model package (data object types, DOT).
                _predefinedCode.Add($"    <Compile Include=\"Model\\DOT\\{baseClassName}.cs\" />");

                // Components of storage.
                _predefinedCode.Add($"    <Compile Include=\"Model\\Storage\\{baseClassName}Storage.cs\" />");

                // Components of UI resources (.resx).
                _predefinedCode.Add($"    <EmbeddedResource Include=\"Gui\\Elements\\DOP{baseClassName}.resx\">");
                _predefinedCode.Add($"      <DependentUpon>{component.Name}</DependentUpon>");
                _predefinedCode.Add($"    </EmbeddedResource>");
            }
            #endregion

            _predefinedCode.Add($"    <Compile Include=\"Gui\\UiRegistry.cs\" />");
            _predefinedCode.Add($"    <Compile Include=\"Model\\Storage\\StorageRegistry.cs\" />");
            _predefinedCode.Add($"    <Compile Include=\"Properties\\AssemblyInfo.cs\" />");
            _predefinedCode.Add($"    <EmbeddedResource Include=\"Properties\\Resources.resx\">");
            _predefinedCode.Add($"      <Generator>ResXFileCodeGenerator</Generator>");
            _predefinedCode.Add($"      <LastGenOutput>Resources.Designer.cs</LastGenOutput>");
            _predefinedCode.Add($"      <SubType>Designer</SubType>");
            _predefinedCode.Add($"    </EmbeddedResource>");
            _predefinedCode.Add($"    <Compile Include=\"Properties\\Resources.Designer.cs\">");
            _predefinedCode.Add($"      <AutoGen>True</AutoGen>");
            _predefinedCode.Add($"      <DependentUpon>Resources.resx</DependentUpon>");
            _predefinedCode.Add($"      <DesignTime>True</DesignTime>");
            _predefinedCode.Add($"    </Compile>");
            _predefinedCode.Add($"  </ItemGroup>");
            _predefinedCode.Add($"  <ItemGroup>");
            _predefinedCode.Add($"    <ProjectReference Include=\"{FileHelper.GetRelativePath(in_projectPackage.FullPath + "\\", cSharpAppTarget.OrmLibProjectDir)}\\{cSharpAppTarget.OrmLibProjectName}.csproj\">");
            _predefinedCode.Add($"      <Project>{{{TargetCSharpAppLegacy.C_ORMLIB_PROJECT_GUID_STRING.ToLower()}}}</Project>");
            _predefinedCode.Add($"      <Name>{cSharpAppTarget.OrmLibProjectName}</Name>");
            _predefinedCode.Add("    </ProjectReference>");
            //if (generator.Target.IsDependantOnSQLite)
            //{
            //    _predefinedCode.Add($"    <ProjectReference Include=\"{FileHelper.GetRelativePath(in_projectPackage.FullPath + "\\", generator.Target.SQLiteProjectFullPath)}\">");
            //    _predefinedCode.Add($"      <Project>{{{TargetSQLite.C_SQLITE_PROJECT_GUID.ToString()}}}</Project>");
            //    _predefinedCode.Add($"      <Name>orm_sqlite</Name>");
            //    _predefinedCode.Add("    </ProjectReference>");
            //}
            _predefinedCode.Add($"  </ItemGroup>");
            _predefinedCode.Add($"  <ItemGroup />");
            _predefinedCode.Add($"  <Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />");
            _predefinedCode.Add($"  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. ");
            _predefinedCode.Add($"       Other similar extension points exist, see Microsoft.Common.targets.");
            _predefinedCode.Add($"  <Target Name=\"BeforeBuild\">");
            _predefinedCode.Add($"  </Target>");
            _predefinedCode.Add($"  <Target Name=\"AfterBuild\">");
            _predefinedCode.Add($"  </Target>");
            _predefinedCode.Add($"  -->");
            _predefinedCode.Add($"</Project>");
        }

        public new CSharpProjectBase Package { get { return (CSharpProjectBase)base.Package; } }
    }
}
