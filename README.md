# VkRadio.LowCode
My old low-code toolset (full publishing is not finished yet).

# Structure and internal dependencies

*VkRadio.LowCode.AppGen.Domain* - the domain (entities) model, they are being mapped to both SQL database structure,
layers in a programming language, and their interactions (like SQL queries, UI bindings, etc.).

*VkRadio.LowCode.AppGen.ArtefactGenerators.Core* - the abstract representation of an artefact type that is being derived
from the Domain.

*VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore* - common SQL-related structures and logic.

*VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore* - common object-oriented language structures and logic.

*VkRadio.LowCode.AppGen.ArtefactGenerators.MsSql* - generator of MS SQL Server compatible SQL database schema.

*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.Core* - basic structures and logic for C# code generator.

*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.Entities* - generator of entities defined in C#.

*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.Entities.Storage* - generator of read/write operations within C#
entities and SQL tables.

*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.GUI.WinForms* - generator of WinForms GUI elements with bindings
to entities and storages.

*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.Core* - basic structures and loginc for representation and work
with Artefaction generation targets.

*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.MsSql* - representation of a generated package: MS SQL database
schema.

*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.CSharp.WinFormsApp* - representation of a generated package:
WinForms App

*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.CSharp.EntitiesAndStorage* - representation of a generated package:
only entities and storage, without GUI (so it can be used as a domain base in progects without WinForms, such as Web Apps).

*VkRadio.LowCode.AppGen.App* - DLL with the app logic.

*VkRadio.LowCode.AppGen.AppConsole* - thin console EXE wrapper on top of an App.


# External dependencies

*VkRadio.LowCode.Orm* (.NET Standard 2.0) - Object-relational mapper

*VkRadio.LowCode.Orm.MsSql* (.NET Standard 2.0) - MS SQL Server specifics for an ORM library
