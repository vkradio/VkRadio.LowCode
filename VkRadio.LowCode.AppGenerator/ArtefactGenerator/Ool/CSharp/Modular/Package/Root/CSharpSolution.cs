using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component;
using ArtefactGenerationProject.ArtefactGenerator.Sql;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Root
{
    public class CSharpSolution: ProjectPackage
    {
        /// <summary>
        /// Increment ProjectGuid of &quot;base&quot; project to generate ProjectGuid
        /// of &quot;extension&quot; project
        /// </summary>
        /// <param name="guid">ProjectGuid of &quot;base&quot; project</param>
        /// <returns>ProjectGuid of &quot;extension&quot; project</returns>
        Guid SimpleIncrementGuid(Guid guid)
        {
            var extIdBytes = guid.ToByteArray();
            var byteVal = extIdBytes[extIdBytes.Length - 1];
            byteVal++;
            extIdBytes[extIdBytes.Length - 1] = (byte)(byteVal % 255);
            return new Guid(extIdBytes);
        }

        /// <summary>
        /// Open constructor for partial pre-initializing
        /// </summary>
        /// <param name="in_cSharpGenerator">C# artefacts generator</param>
        /// <param name="in_dbSchemaModel">Database schema model</param>
        public CSharpSolution(ArtefactGeneratorCSharpModular in_cSharpGenerator, DBSchemaMetaModelJson in_dbSchemaModel)
            : base(in_cSharpGenerator.Target.Project.DomainModel, in_cSharpGenerator.Target, in_dbSchemaModel)
            => Generator = in_cSharpGenerator;

        /// <summary>
        /// Initializing after creation for concrete class
        /// </summary>
        public override void Init()
        {
            var projectId = ArtefactGenerationTarget.Parent.Id;
            BaseProject = new CSharpProjectBase(this, projectId);
            _subpackages.Add(BaseProject.Name, BaseProject);

            //projectId = SimpleIncrementGuid(projectId);
            //ExtensionProject = new CSharpProjectExtension(this, projectId);
            //_subpackages.Add(ExtensionProject.Name, ExtensionProject);

            MiniSolutionDescriptor = new Solution(this);
            _components.Add(MiniSolutionDescriptor.Name, MiniSolutionDescriptor);
        }

        public new TargetCSharpSolution ArtefactGenerationTarget { get => (TargetCSharpSolution)base.ArtefactGenerationTarget; }
        public ArtefactGeneratorCSharpModular Generator { get; private set; }
        public Solution MiniSolutionDescriptor { get; private set; }
        public CSharpProjectBase BaseProject { get; private set; }
        //public CSharpProjectExtension ExtensionProject { get; private set; }
    };
}
