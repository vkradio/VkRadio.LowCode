# VkRadio.LowCode
My old low-code toolset (full publishing is not finished yet).

# Structure and internal dependencies

*VkRadio.LowCode.AppGen.Domain* - the domain (entities) model, they are being mapped to both SQL database structure,
layers in a programming language, and their interactions (like SQL queries, UI bindings, etc.).

*VkRadio.LowCode.AppGen.ArtefactGenerators.Core* - the abstract representation of an artefact type that is being derived
from the Domain.

*VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core* - common SQL-related structures and logic.

*VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql* - generator of MS SQL Server compatible SQL database schema.

*VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core* - common object-oriented language structures and logic.

*VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core* - common s and logic for C# projects.

*VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinForms* - monolithic C# app project for WinForms and .NET Framework 4.7

*VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Modular* - modular C# project for .NET Standard 2.0

//*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.Core* - basic structures and logic for C# code generator.

//*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.Entities* - generator of entities defined in C#.

//*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.Entities.Storage* - generator of read/write operations within C#
//entities and SQL tables.

//*VkRadio.LowCode.AppGen.ArtefactGenerators.CSharp.GUI.WinForms* - generator of WinForms GUI elements with bindings
//to entities and storages.

//*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.Core* - basic structures and loginc for representation and work
//with Artefaction generation targets.

//*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.MsSql* - representation of a generated package: MS SQL database
//schema.

//*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.CSharp.WinFormsApp* - representation of a generated package:
//WinForms App

//*VkRadio.LowCode.AppGen.ArtefactGenerationTargets.CSharp.EntitiesAndStorage* - representation of a generated package:
//only entities and storage, without GUI (so it can be used as a domain base in progects without WinForms, such as Web Apps).

*VkRadio.LowCode.AppGen.App* - DLL with the app logic.

*VkRadio.LowCode.AppGen.AppConsole* - thin console EXE wrapper on top of an App.


# External dependencies

*VkRadio.LowCode.Orm* (.NET Standard 2.0) - Object-relational mapper

*VkRadio.LowCode.Orm.MsSql* (.NET Standard 2.0) - MS SQL Server specifics for an ORM library

# Misc considerations about legacy project design (should be reviewed and updated according to modern design practices)

1. Console app: Loads the project file

1.1. Load each target description

2. Console app: Generate artefacts for each target

2.1. Target - calls a cocrete Artefact Generator to generate artefacts for itself

Here Target creates an Artefact Generator upon project load and saves it inside its properties. Generator inself also
has link to its Target - to have an ability to extract Target dependencies (and their generated results).

Initially the Target class was common for all Artefact Generators, and concrete settings were stored in concrete Generators,
but then concrete Targets were introduced, so mix of settings between Targets and Generates introduced over-complications.
Essentially, from the Project point of view the Target is a "passive" bag of settings, and the Generator is an "active"
process that reads its Target properties and produces artifacts. But from the OOP point of view, it could be considered
as a single object with properties and procedures. Let for starters simplify a structure a bit - remove all concrete properties
from Target and make it to be a single class back, and move all concrete properties to corresponding concrete Generators.
