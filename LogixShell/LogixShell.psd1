@{
    RootModule = 'LogixShell.dll'
    ModuleVersion = '1.0.0'
    CompatiblePSEditions = @('Core')
    GUID = 'f17eadeb-c394-4aed-b13c-6e2e3a2b074d'
    Author = 'Timothy Nunnink'
    Description = 'PowerShell Module for working with Logix project files'
    PowerShellVersion = '7.0'
    RequiredAssemblies = @(
        'L5Sharp.Core.dll'
    )
    FunctionsToExport = '*'
    CmdletsToExport = '*'
    FormatsToProcess = @('LogixShell.Format.ps1xml')
    AliasesToExport = @()
    PrivateData = @{
        PSData = @{
            Tags = @('Logix', 'Rockwell', 'Automation', 'L5X')

            # A URL to the license for this module.
            # LicenseUri = ''

            # A URL to the main website for this project.
            # ProjectUri = ''

            # A URL to an icon representing this module.
            # IconUri = ''

            # ReleaseNotes of this module
            # ReleaseNotes = ''
        }
    }
}
