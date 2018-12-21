# Dynamo_Toolkit
BHoM Dynamo Tools

There have been some fairly major updates to the Basilisk project recently, to make it more flexible for future development. 

### Build Dynamo_Toolkit from Source ###
You will need the following to build Dynamo_Toolkit:

- Microsoft Visual Studio 2013 or higher
- [GitHub for Windows](https://windows.github.com/)
- Microsoft .NET Framework 4.0 and above (included with Visual Studio 2013)
- BHoM version 0.0.1
- RobotToolkit version 0.0.1
- Ensure post-build folders are accessible:
    - Dynamo_Toolkit (zero touch essentials) node library copies to C:\Users\$(Username)\AppData\Roaming\Dynamo\0.9\packages\BH Dynamo Tools. If you need the package folder that includes the bin folder and pkg.json file, copy from the Dynamo_Toolkit Plugins folder.
    - Dynamo_Toolkit UI nodes library copies to C:\Program Files\Dynamo 0.9\nodes on build. This folder is protected by default - you'll need to allow write access with local rights.

## Contribute ##

Basilisk is a BuroHappold open-source project and would be nothing without its community.  You can submit your own code to the Basilisk project via a Github [pull request](https://help.github.com/articles/using-pull-requests).

## Releases ##
###0.0.1 ###
Known Issues
 - BHoM Project Guid is not yet traced, this will be implemented soon so that by default, all BHoM objects created in a workspace will be created in a single project (instantiated at the creation of the first BHoM object in the workspace). A 'switch project' node will be added so a user can change the project that an object is associated with manually if multiple projects are required in the same workspace.

Bug fixes
 - Drop down nodes now no longer cause a crash

New features
 - BasiliskNodesUI - this project is set up as a framework to create nodes with custom user interfaces, for example, drop down nodes nodes for BHoM object property deconstructore
 - New BHoM structural element factory creation - objects must now be created using the Factory rather than independently (they must belong to a BHoM.Global.Project). 


## License ##

Basilisk is licensed under the [LGPL](https://github.com/BHoM/Dynamo_Toolkit/blob/master/LICENSE) License. Basilisk also uses a number of third party libraries, some with different licenses.
