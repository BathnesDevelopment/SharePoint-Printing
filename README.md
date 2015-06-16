# SharePoint-Printing

  C# App to Print documents from a list in SharePoint.

## Use
  The user selects the documents from the list they want to print and selects "print". This uses the default windows print verb for that file type.

## Build instructions
  Load into Visual Studio and edit the siteURL and list in the app.config file. Build and Deploy with Visual Studio.

## SharePoint configuration
  The app runs off a customised list with a reference to the URL of the document, and workflows to remove the item from the list with the delete button. The print button also has a workflow associated for logging reasons. If you want to build a similar app using this as a base, you'll need to add these workflows or remove the workflow parts of the code.
