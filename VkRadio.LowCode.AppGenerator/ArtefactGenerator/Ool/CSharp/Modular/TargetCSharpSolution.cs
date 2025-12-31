using Newtonsoft.Json;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular
{
    public class TargetCSharpSolution : ArtefactGenerationTargetJson
    {
        /// <summary>
        /// Place all generated classes of same kind to single file component.
        /// Otherwise there will be sigle file per DOT, or even few files per DOT.
        /// </summary>
        [JsonProperty]
        public bool AllDOTsInSingleFile { get; set; }

        protected override void CreateGenerator() => Generator = new ArtefactGeneratorCSharpModular(this);

        public bool IsDependantOnSQLite { get => false; }
        public string SQLiteProjectFullPath { get => null; }
        public new TargetCSharpApp Parent { get => (TargetCSharpApp)base.Parent; }
        public new ArtefactGeneratorCSharpModular Generator { get => (ArtefactGeneratorCSharpModular)base.Generator; set => base.Generator = value; }
    };
}
