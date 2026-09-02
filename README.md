# Greater Tzaneen Municipality Services Application

## 1. Project Overview

The Greater Tzaneen Municipality Services Application is a C# Windows
Forms application developed for Part 1 of the Portfolio of Evidence
(PoE).

The purpose of the application is to provide residents with a simple and
user-friendly way to report municipal issues and service-related
problems. Part 1 focuses on the **Report Issues** functionality.

The Main Menu provides three options:

-   **Report Issues** -- implemented in Part 1.
-   **Local Events and Announcements** -- disabled and reserved for a
    later part.
-   **Service Request Status** -- disabled and reserved for a later
    part.

## 2. Technologies Used

-   C#
-   .NET Framework
-   Windows Forms
-   Microsoft Visual Studio 2022
-   Git/GitHub for version control

## 3. Main Features

### Main Menu

When the application starts, the user is presented with the three
required municipal service options. Only **Report Issues** is active in
Part 1.

### Report Issues

The Report Issues form allows residents to:

1.  Enter the location of an issue.
2.  Select an issue category.
3.  Enter a detailed description.
4.  Attach an image or document.
5.  Submit the report.
6.  Return to the Main Menu.

### Issue Categories

The category selection may include:

-   Roads
-   Sanitation
-   Water
-   Electricity
-   Waste/Refuse
-   Other

### File Attachments

An `OpenFileDialog` is used to allow users to select an image or
document related to the reported issue.

### User Engagement

The application includes an engagement feature such as progress or
encouraging feedback to encourage residents to complete and submit
reports. This approach is informed by Hart, Booyens and Sinyolo (2020),
who emphasise active participation, collaboration and consideration of
local community needs in municipal innovation.

## 4. Data Handling

A C# `List` is used to store reported issues during application use.
Each issue contains relevant information such as:

-   Location
-   Category
-   Description
-   Attachment information

The Part 1 implementation stores the information during the current
application session. Additional persistent storage can be added in
future parts if required.

## 5. Requirements

The following should be installed before running the application:

-   Windows operating system
-   Microsoft Visual Studio 2022
-   .NET Framework development tools
-   C# Windows Forms development workload

## 6. How to Compile

1.  Open **Microsoft Visual Studio 2022**.
2.  Open the application's `.sln` solution file.
3.  Allow the project to load.
4.  Select **Build \> Build Solution**.
5.  Check the Output/Error List for errors.
6.  If the build is successful, the application is ready to run.

## 7. How to Run

1.  Open the solution in Visual Studio 2022.
2.  Press **F5** or select **Start**.
3.  The Main Menu will appear.
4.  Select **Report Issues**.
5.  Complete the required fields.
6.  Attach an image or document if required.
7.  Select **Submit Report**.
8.  Review the confirmation or validation message.
9.  Select **Back to Main Menu** to return to the Main Menu.

## 8. Using the Report Issues Form

### Location

Enter the location where the problem occurred.

Example:

`123 Main Street, Tzaneen`

### Category

Select the category that best describes the issue.

Example:

`Roads`

### Description

Provide a clear description of the problem.

Example:

`There is a large pothole near the entrance to the residential area.`

### Attachment

Select **Attach Image / Document** and choose a relevant file from the
computer.

### Submit

Select **Submit Report** after completing the required information. The
application provides feedback when the report is successfully submitted
or when required information is missing.

## 9. Validation and User Feedback

Validation is used to help prevent incomplete reports from being
submitted. Users are notified when required information such as
location, category or description has not been entered.

`MessageBox` notifications are used to communicate successful
submissions, errors and other important information.

## 10. Navigation

The **Back to Main Menu** button allows users to return to the Main Menu
easily.

## 11. Future Development

The application will be expanded in later parts of the Portfolio of
Evidence. Possible future functionality includes:

-   Local Events and Announcements
-   Service Request Status
-   Database/persistent storage
-   Service request tracking
-   Additional citizen engagement features

## 12. Troubleshooting

### Application does not build

-   Confirm that Visual Studio 2022 is installed.
-   Confirm that the required .NET Framework is installed.
-   Select **Build \> Rebuild Solution**.
-   Check the Error List for compiler errors.

### Application does not start

-   Confirm that the correct project is selected as the Startup Project.
-   Rebuild the solution.
-   Run the application using **F5**.

### Attachment does not work

-   Check that the `OpenFileDialog` is configured correctly.
-   Check that the selected file exists and can be accessed.
-   Test with a common image or document file.

## 13. Version Control

Git is used to track development changes.

The final Part 1 commit is:

`Complete Part 1 Municipal Services Application`

Keep a backup of the project before beginning future parts of the
Portfolio of Evidence.

## 14. Author

**Student:** \[ST10380589]\
**Course/Module:** \[ADAD\]\
**Project:** Municipal Services Application -- Part 1\
**Municipality:** Greater Tzaneen Municipality\
**Development Environment:** Visual Studio 2022\
**Language:** C#

## 15. Research Source

Hart, T.G.B., Booyens, I. and Sinyolo, S. (2020) 'Innovation for
development in South Africa: experiences with basic service technologies
in distressed municipalities', *Forum for Development Studies*, 47(1),
pp. 23--47. doi:10.1080/08039410.2019.1654543.

This source informed the application's user engagement approach,
particularly the importance of active community participation,
collaboration and consideration of local needs in municipal service
delivery.
