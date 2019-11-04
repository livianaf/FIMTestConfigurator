# FIMTestConfigurator - Automating FIM/MIM Tests

## Audience
This tool and its description is intended only for advanced [FIM/MIM](https://en.wikipedia.org/wiki/Forefront_Identity_Manager) users and developers. To understand how this works, you should be used to a specific terminology (whose explanation is outside the scope of this document).
## Downloads
* [FIMTestConfigurator](https://github.com/livianaf/FIMTestConfigurator/releases/tag/v1.0.1)
* [FIMTestsRunner](https://github.com/sfalbacete/FIMTestsRunner/releases)
## Introduction
FIM is the Microsoft Identity Model Synchronization Engine, which is specifically called "Forefront Identity Manager".
This tool allows you to define groups of **synchronization tests** to be executed with [SFAlbacete](https://github.com/sfalbacete)'s [FIMTestsRunner](https://github.com/sfalbacete/FIMTestsRunner). For more information about tests execution, it is highly recommended to visit [SFAlbacete's hideout](https://github.com/sfalbacete).

## The concept
We, as FIM developers, needed a tool to automate all the possible "use-cases" (mostly [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete)-oriented) related to the FIM Rules Extensions created during our projects. With every new version of the Rules Extensions libraries (DLLs), we needed to make sure everything kept working as expected. In order to avoid running into problems, we decided to develop a small tool to allow us to execute some basic tests and figure out if the new libraries would work well.

## What is a FIM Test?
Generally speaking, a FIM Test is in charge for taking care about what happens during the **inbound and outbound synchronization** of an **identity**. An identity is usually refered to a person data (name, surname, address, etc).

Without going into much detail, synchronization happens in two stages: **inbound** and **outbound** sync. This means that FIM reads data from the inbound (or source) management agent, and sends it to the outbound (or destination) management agents. The test keeps track of the data being imported and exported and checks for its validity. And that's pretty much it.
However, Microsoft defines the synchronization lifecycle which is documented, referred to the [*IMASynchronization*](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/identity-lifecycle-manager/ms696509(v%3Dvs.85)) and [*IMVSynchronization*](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/identity-lifecycle-manager/ms696525(v%3Dvs.85)) interfaces.

For each step, you can define the rules (assertions/checks) that the FIM Runner tries to verify. This is what we call "Output Sets".

A FIM Test DB file contains the following elements one or more of following elements:
- **Batch**: this is just a grouping element and this is the higher one. A Batch contains Groups.
- **Group**: this is a grouping element. A Group contains a sorted list of Tests.
- **Test**: a test definition contains all required information to execute a test in FIM/MIM. The anatomy of a FIM Test contains the following elements: 
  - **Config** (one optional): this points to a configuration file just in case your solution uses one!. This way, you could mimic a different configuration for each tests.
  - **Script** (one optional): this allows you to execute some PS scripting before and after the test. It is usefull to prepare things before the test, and to dispose them after it.
  - **Source MA** (one): The inbound management agent.
    - **Source** (one): The credential required by the management agent.
  - **Input Set** (one): It contains a DN (which is a reference to an identity in a directory), and a set of input values to be applied to the attributes of that object before the test.
  - **Output Sets** (one or more): It contains a set of conditions regarding the synchronization lifecycle itself. An output set responds to a synchronization event. We can do assertions for several events: filtering, projection, join, import flow, provision, deprovision, export flow, etc.
- **Variables** (optional): Variables (if defined) are used during test execution. The list of variable names is unique in each DB test file. A list of variable values is configured for a *computer name*. You can define several set of values for diferent *computer names*. The [FIMTestsRunner](https://github.com/sfalbacete/FIMTestsRunner) application use the set of values defined for the *computer name* where it is running.
## Tests, Groups and Batches
Going straight to the point, a group contains several tests, and a batch contains several groups. This way you can structure your tests whatever you prefer and selectively run them.

## Executing tests
As mentioned before, you need another tool, [FIMTestsRunner](https://github.com/sfalbacete/FIMTestsRunner) in order to be able to execute the tests defined in the .db file.
This tool is launched when you click on the "Run Tests" button in the upper toolbar.

## Basic operations
The menu bar contains the following commands:
- **Open Test File**. This command open and existing DB Test file or create a new test db file with the path provided in the dialog window.
- **Show/Hide View**. This command allow to hide items in the left tree view panel and unhide items previously hidden.
- **check Intergrity**. This command validate configuration of each item defined in the DB test file. 
- **Edit Variables**. This command show an additional dialog window used to define variables and values used during test execution.
- **Close DB**. This command is used to close active DB file. 
- **Find**. This box is used to search by text in all visible items defined in active DB file. Use CTRL+F to activate the search text box and F3 to search occurences.
- **Run Tests**. This command open [FIMTestsRunner](https://github.com/sfalbacete/FIMTestsRunner) application.
- <**Additional External Tools**>. The last three commands are configurable. They are defined in section **appSettings** of **FIMTestConfigurator.exe.Config** file. The **key** must be `ExtTool1`, `ExtTool2` or `ExtTool3`. The **value** must contains three values joined by "|" character: `<text to display>|<path to the application>|<parameters>`. If **value** is empty the command button is not shown. If **parameters** section contains **#DB#** it is replaced by active test DB file. Sample:
```
    <add key="ExtTool1" value="FIM Sync|D:\Util\FIMSyncTool\FIMSyncTest.exe|" />
    <add key="ExtTool2" value="SQLite DB Browser|D:\Util\SQLiteDBBrowser\SQLiteDBBrowser.exe|#DB#" />
    <add key="ExtTool3" value="FIM Config Files|D:\Util\Notepad++\notepad++.exe|-multiInst -nosession &quot;D:\Synchronization Service\Extensions\config*.xml&quot;  &quot;D:\Synchronization Service\Extensions\config*.cfg&quot;" />
 ```


## Implementation
It is written in C#, and contains the following projects:
- **FIMTestConfigurator**. A Windows Forms project type. It contains the  forms and controls required by the application.
- **FIMTestWpfConfigurator**. A WPF project type. It is an alternative implementation to the first one.
- **FIMTestconfigLib**. A Class Library project type, being used by the preevisous projects.

## External dependencies
- **System.Data.SQLite Interop Library** 1.0.65.0. Used to read the defined tests stored in a .db file.

*Note*: all dependency files are included in the releases so you can take the DLLs in order to be able to compile the project.
