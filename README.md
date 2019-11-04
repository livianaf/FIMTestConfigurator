# FIMTestConfigurator - Automating FIM Tests

## Audience
This tool and its description is intended only for advanced [FIM/MIM](https://en.wikipedia.org/wiki/Forefront_Identity_Manager) users and developers. To understand how this works, you should be used to a specific terminology (whose explanation is outside the scope of this document).

## Introduction
FIM is the Microsoft Identity Model Synchronization Engine, which is specifically called "Forefront Identity Manager".
This tool allows you to define groups of **synchronization tests** to be executed with [SFAlbacete's "FIMTestsRunner"](https://github.com/sfalbacete/FIMTestsRunner). For more information about tests execution, it is highly recommended to visit [SFAlbacete's hideout](https://github.com/sfalbacete).

## The concept
We, as FIM developers, needed a tool to automate all the possible "use-cases" (mostly [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete)-oriented) related to the FIM Rules Extensions created during our projects. With every new version of the Rules Extensions libraries (DLLs), we needed to make sure everything kept working as expected. In order to avoid running into problems, we decided to develop a small tool to allow us to execute some basic tests and figure out if the new libraries would work well.

## What is a FIM Test?
Generally speaking, a FIM Test is in charge for taking care about what happens during the **inbound and outbound synchronization** of an **identity**. An identity is usually refered to a person data (name, surname, address, etc).

Without going into much detail, synchronization happens in two stages: **inbound** and **outbound** sync. This means that FIM reads data from the inbound (or source) management agent, and sends it to the outbound (or destination) management agents. The test keeps track of the data being imported and exported and checks for its validity. And that's pretty much it.
However, Microsoft defines the synchronization lifecycle which is documented, referred to the [*IMASynchronization*](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/identity-lifecycle-manager/ms696509(v%3Dvs.85)) and [*IMVSynchronization*](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/identity-lifecycle-manager/ms696525(v%3Dvs.85)) interfaces.

For each step, you can define the rules (assertions/checks) that the FIM Runner tries to verify. This is what we call "Output Sets".

The anatomy of a FIM Test contains the following elements:
- **Config** (optional): this points to a configuration file just in case your solution uses one!. This way, you could mimic a different configuration for each tests.
- **Script** (optional): this allows you to execute some PS scripting before and after the test. It is usefull to prepare things before the test, and to dispose them after it.
- **Source MA**: The inbound management agent.
- **Input Set**: It contains a DN (which is a reference to an identity in a directory), and a set of input values to be applied to the attributes of that object before the test.
- **Output Sets**: It contains a set of conditions regarding the synchronization lifecycle itself. An output set responds to a synchronization event. We can do assertions for several events: filtering, projection, join, import flow, provision, deprovision, export flow, etc.

## Tests, Groups and Batches
Going straight to the point, a group contains several tests, and a batch contains several groups. This way you can structure your tests whatever you prefer and selectively run them.

## Executing tests
As mentioned before, you need another tool, "FIMTestsRunner" in order to be able to execute the tests defined in the .db file.
This tool is launched when you click on the "Run Tests" button in the upper toolbar.

## Implementation
It is written in C#, and contains two projects:
- **FIMTestConfigurator**. A Windows Forms project type. It contains the  forms and controls required by the application.
- **FIMTestWpfConfigurator**. A WPF project type. It is an alternative implementation to the first one.
- **FIMTestconfigLib**. A Class Library project type, being used by the preevisous projects.

## External dependencies
- **System.Data.SQLite Interop Library** 1.0.65.0. Used to read the defined tests stored in a .db file.

Note: all dependency files are included in the releases so you can take the DLLs in order to be able to compile the project.
