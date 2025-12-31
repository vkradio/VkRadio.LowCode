using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp2;
using ArtefactGenerationProject.ArtefactGenerator.Sql;
using ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtefactGenerationProject.ArtefactGenerator
{
    public abstract class ArtefactGenerator
    {
        public ArtefactGenerationTarget Target { get; private set; }

        //public MetaModel.MetaModel MetaModel { get; private set; }

        public ArtefactGenerator(ArtefactGenerationTarget target) => Target = target;

        /// <summary>
        /// Генерирование пакета артефактов
        /// </summary>
        public abstract string Generate();

        /// <summary>
        /// Создание экземпляра генератор артефактов конкретного типа
        /// </summary>
        /// <param name="in_target">Цель генерации</param>
        /// <param name="in_metaModel">Метамодель</param>
        /// <returns>Генератор артефактов</returns>
        public static ArtefactGenerator CreateConcrete(ArtefactGenerationTarget in_target, MetaModel.MetaModel in_metaModel)
        {
            ArtefactGenerator generator;

            switch (in_target)
            {
                case TargetMsSql targetSql:
                    generator = new ArtefactGeneratorSql(null);
                    break;
                //case ArtefactTypeCodeEnum.PhpZf:
                //    generator = new ArtefactGeneratorPhpZf();
                //    break;
                case GenerationTargetCSharp targetCSharp:
                    generator = new ArtefactGeneratorCSharp(null);
                    break;
                //case TargetCSharpApplication targetCSharpApplication:
                //    generator = new ArtefactGeneratorCSharpApplication(null);
                //    break;
                //case TargetCSharpProjectModel targetCSharpProjectModel:
                //    generator = new ArtefactGeneratorCSharpProjectModel(null);
                //    break;
                //case ArtefactTypeCodeEnum.CSharpOldVersionSave:
                //    generator = new ArtefactGeneratorCSharpOldVersionSave() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
                //    break;
                //case ArtefactTypeCodeEnum.CSharpProjectVersion:
                //    generator = new ArtefactGeneratorCSharpProjectVersion() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
                //    break;
                //case ArtefactTypeCodeEnum.InnoSetup:
                //    generator = new ArtefactGeneratorInnoSetup() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
                //    break;
                //case ArtefactTypeCodeEnum.MSBuild:
                //    generator = new ArtefactGeneratorMSBuild() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
                //    break;
                default:
                    throw new ApplicationException($"Unrecognized target type: {in_target.GetType().Name}.");
            }
            //generator.MetaModel = in_metaModel;
            //generator.Target = in_target;

            return generator;
        }
    }
}
