using System.Xml.Linq;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

public abstract class PFTDependentLink : PFTLink
{
    protected abstract void SetDefaultOnDeleteAction();

    public OnDeleteActionEnum OnDeleteAction { get; protected set; }

    protected PFTDependentLink(bool? nullable, bool? unique, XElement xElement, DomainModel domainModel)
        : base(nullable, unique, xElement)
    {
        GuardRule.Forbid.Null(domainModel, nameof(domainModel));

        var xel = xElement.Element("OnRefObjectDelete");

        if (xel is not null)
        {
            OnDeleteAction = xel.Value switch
            {
                "ignore" => OnDeleteActionEnum.Ingnore,

                "delete" => OnDeleteActionEnum.Delete,

                "block" => OnDeleteActionEnum.CannotDelete,

                "set default value" => OnDeleteActionEnum.ResetToDefault,

                "set null" => OnDeleteActionEnum.ResetToNull,

                _ => throw new ApplicationException(string.Format(
                    "Property {0} has unsupported OnRefObjectDelete value: {1}.",
                    xElement.Element("Id") is not null
                        ? xElement.Element("Id")!.Value
                        : "<NULL>",
                    xel.Value ?? "<NULL>"
                ))
            };
        }
        else
        {
            if (domainModel.DefaultLinksStrict)
            {
                if (Nullable)
                {
                    OnDeleteAction = OnDeleteActionEnum.ResetToNull;
                }
                else
                {
                    SetDefaultOnDeleteAction();
                }
            }
        }
    }
}
