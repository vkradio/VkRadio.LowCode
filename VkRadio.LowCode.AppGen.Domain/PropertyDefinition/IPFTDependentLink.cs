namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

public interface IPFTDependentLink
{
    OnDeleteActionEnum OnDeleteAction { get; set; }

    void SetDefaultOnDeleteAction();
}
