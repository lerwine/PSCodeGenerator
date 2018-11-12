<#
foreach ($Name in [Enum]::GetNames([System.Reflection.TypeAttributes])) {
    (-join (&{
        $v = [Enum]::Parse([System.Reflection.TypeAttributes], $Name);
        for ($i = 31; $i -ge 0; $i--) {
            if (((1 -shl $i) -band $v) -eq 0) { "0" } else { "1" }
        }
    })) + " " + $Name;
}
#>

Function New-CodeCompileUnit {
    [OutputType([System.CodeDom.CodeCompileUnit])]
    Param()
    
    New-Object -TypeName 'System.CodeDom.CodeCompileUnit';
}

Function Test-LanguageIndependentIdentifier {
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [string[]]$InputString,

        [switch]$AllowNamespace
    )

    Process {
        if ($null -eq $InputString) {
            $false | Write-Output;
        } else {
            if ($AllowNamespace) {
                foreach ($ns in $InputString) {
                    $ns -ne $null -and $ns.Trim().Length -gt 0 -and @(($ns | ForEach-Object { $_.Split('.') | Where-Object { $_.Trim().Length -eq 0 -or -not [System.CodeDom.Compiler.CodeGenerator]::IsValidLanguageIndependentIdentifier($_) } })).Count -eq 0;
                }
            } else {
                foreach ($ns in $InputString) { $ns -ne $null -and $ns.Trim().Length -gt 0 -and [System.CodeDom.Compiler.CodeGenerator]::IsValidLanguageIndependentIdentifier($ns) }
            }
        }
    }
}

Function Test-CodeNamespace {
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [ValidateScript({ $_ -eq $null -or $_ -is [string] -or $_ -is [System.CodeDom.CodeNamespace] })]
        [object[]]$Namespace
    )

    Process {
        if ($null -eq $Namespace) {
            $false | Write-Output;
        } else {
            foreach ($ns in $Namespace) {
                if ($null -eq $ns) {
                    $false | Write-Output;
                } else {
                    if ($ns -is [string]) {
                        $ns.Length -eq 0 -or ($ns | Test-LanguageIndependentIdentifier -AllowNamespace);
                    } else {
                        $ns.Name.Length -eq 0 -or ($ns.Name | Test-LanguageIndependentIdentifier -AllowNamespace);
                    }
                }
            }
        }
    }
}

Function Find-CodeNamespace {
    [OutputType([System.CodeDom.CodeNamespace])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.CodeDom.CodeCompileUnit[]]$CompileUnit,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [ValidateScript({ $_ -is [string] -or $_ -is [System.CodeDom.CodeNamespace] })]
        [string[]]$Namespace
    )

    Process {
        $n = '';
        foreach ($c in ($CompileUnit | Where-Object { $_.Namespaces.Count -gt 0 })) {
            foreach ($ns in $Namespace) {
                $c.Namespaces | Where-Object { $_.Name -ceq $ns }
            }
        }
    }
}

Function Add-CodeNamespace {
    [OutputType([System.CodeDom.CodeNamespace])]
    Param(
        [Parameter(Mandatory = $true)]
        [System.CodeDom.CodeCompileUnit]$CompileUnit,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ $_ | Test-CodeNamespace })]
        [AllowEmptyString()]
        [string[]]$Namespace

    )

    Process {
        $Namespace | ForEach-Object {
            $ns = $CompileUnit | Find-CodeNamespace -Namespace $_;
            if ($null -eq $ns) {
                $ns = New-Object -TypeName 'System.CodeDom.CodeNamespace' -ArgumentList $_;
                $CompileUnit.Namespaces.Add($ns) | Out-Null;
            }
            $ns | Write-Output;
        }
    }
}

Function Test-CodeNamespaceImport {
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [ValidateScript({ $_ -eq $null -or $_ -is [string] -or $_ -is [System.CodeDom.CodeNamespaceImport] })]
        [object[]]$Import
    )

    Process {
        if ($null -eq $Import) {
            $false | Write-Output;
        } else {
            foreach ($ns in $Import) {
                if ($null -eq $ns) {
                    $false | Write-Output;
                } else {
                    if ($ns -is [string]) {
                        $ns | Test-LanguageIndependentIdentifier -AllowNamespace;
                    } else {
                        $ns.Namespace | Test-LanguageIndependentIdentifier -AllowNamespace;
                    }
                }
            }
        }
    }
}

Function Find-CodeNamespaceImport {
    [OutputType([System.CodeDom.CodeNamespaceImport])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string[]]$Import,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.CodeDom.CodeNamespace]$Namespace,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'CompileUnit')]
        [System.CodeDom.CodeCompileUnit]$CompileUnit
    )

    Process {
        if ($PSBoundParameters.ContainsKey('CompileUnit')) {
            $ns = Find-CodeNamespace -CompileUnit $CompileUnit -Namespace '';
            if ($ns -ne $null -and $ns.Imports.Count -gt 0) {
                foreach ($i in $Import) {
                    @($ns.Imports) | Where-Object { $_.Namespace -ceq $i }
                }
            }
        } else {
            if ($Namespace.Imports.Count -gt 0) {
                foreach ($i in $Import) {
                    @($Namespace.Imports) | Where-Object { $_.Namespace -ceq $i }
                }
            }
        }
    }
}

Function Add-CodeNamespaceImport {
    [OutputType([System.CodeDom.CodeNamespaceImport])]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.CodeDom.CodeNamespace]$Namespace,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'CompileUnit')]
        [System.CodeDom.CodeCompileUnit]$CompileUnit,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ $_ | Test-CodeNamespaceImport })]
        [object[]]$Import

    )
    Begin {
        $ns = $Namespace;
        if ($PSBoundParameters.ContainsKey('CompileUnit')) { $ns = Add-CodeNamespace -CompileUnit $CompileUnit -Namespace '' }
    }
    Process {
        $i = $Import | Find-CodeNamespaceImport -Namespace $ns;
        if ($null -eq $i) {
            $i = New-Object -TypeName 'System.CodeDom.CodeNamespaceImport' -ArgumentList $_;
            $ns.Imports.Add($i) | Out-Null;
        }
        $i | Write-Output;
    }
}

Function New-CodeAttributeArgument {
    [OutputType([System.CodeDom.CodeAttributeArgument])]
    Param(
        [Parameter(Mandatory = $true)]
        [System.CodeDom.CodeExpression]$Value,
        
        [string]$Name
    )

    Process {
        if ($PSBoundParameters.ContainsKey('Name')) {
            [System.CodeDom.CodeTypeParameter]::new($Name, $Value);
        } else {
            [System.CodeDom.CodeTypeParameter]::new($Value);
        }
    }
}

Function New-CodeAttributeDeclaration {
    [OutputType([System.CodeDom.CodeAttributeDeclaration])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ $_ -is [System.Type] -or ($_ -is [System.CodeDom.CodeTypeParameter] -and $_ | Test-CodeTypeParameter) -or $_ | Test-CodeTypeReference })]
        [object[]]$Type,
        
        [System.CodeDom.CodeAttributeArgument[]]$Arguments
    )

    Process {
        if ($PSBoundParameters.ContainsKey('Arguments')) {
            $Type | ForEach-Object {
                if ($_ -is [string] -or $_ -is [System.CodeDom.CodeTypeReference]) {
                    New-Object -TypeName 'System.CodeDom.CodeAttributeDeclaration' -ArgumentList $_, (,$Arguments);
                } else {
                    if ($_ -is [System.CodeDom.CodeTypeParameter]) {
                        New-Object -TypeName 'System.CodeDom.CodeAttributeDeclaration' -ArgumentList (New-CodeTypeReference -TypeParameter $_), (,$Arguments);
                    } else {
                        New-Object -TypeName 'System.CodeDom.CodeAttributeDeclaration' -ArgumentList (New-CodeTypeReference -Type $_), (,$Arguments);
                    }
                }
            }
        } else {
            $Type | ForEach-Object {
                if ($_ -is [string] -or $_ -is [System.CodeDom.CodeTypeReference]) {
                    New-Object -TypeName 'System.CodeDom.CodeAttributeDeclaration' -ArgumentList $_;
                } else {
                    if ($_ -is [System.CodeDom.CodeTypeParameter]) {
                        New-Object -TypeName 'System.CodeDom.CodeAttributeDeclaration' -ArgumentList (New-CodeTypeReference -TypeParameter $_);
                    } else {
                        New-Object -TypeName 'System.CodeDom.CodeAttributeDeclaration' -ArgumentList (New-CodeTypeReference -Type $_);
                    }
                }
            }
        }
    }
}

Function Test-CodeTypeParameter {
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [ValidateScript({ $_ -eq $null -or $_ -is [string] -or $_ -is [System.CodeDom.CodeTypeParameter] })]
        [object[]]$TypeParameter
    )

    Process {
        if ($null -eq $TypeParameter) {
            $false | Write-Output;
        } else {
            foreach ($p in $TypeParameter) {
                if ($null -eq $p) {
                    $false | Write-Output;
                } else {
                    if ($p -is [string]) {
                        $p | Test-LanguageIndependentIdentifier;
                    } else {
                        $p.Name | Test-LanguageIndependentIdentifier;
                    }
                }
            }
        }
    }
}

Function New-CodeTypeParameter {
    [OutputType([System.CodeDom.CodeTypeParameter])]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-LanguageIndependentIdentifier })]
        [string]$Name,
        
        [Parameter(ValueFromPipeline = $true)]
        [ValidateScript({ $_ -is [System.Type] -or $_ | Test-CodeTypeReference })]
        [object[]]$Constraint,
        
        [Parameter(Mandatory = $true)]
        [System.CodeDom.CodeAttributeDeclaration[]]$CustomAttribute,
        
        [switch]$HasConstructorConstraint
    )

    Begin {
        $CodeTypeParameter = New-Object -TypeName 'System.CodeDom.CodeTypeParameter' -ArgumentList $Name;
    }

    Process {
        if ($PSBoundParameters.ContainsKey('Constraint')) { $CodeTypeParameter.Constraints.AddRange([System.CodeDom.CodeTypeReference[]]($Constraint | New-CodeTypeReference)) }
    }

    End {
        if ($PSBoundParameters.ContainsKey('CustomAttribute')) { $CodeTypeParameter.CustomAttributes.AddRange($CustomAttribute) }
        if ($HasConstructorConstraint) { $CodeTypeParameter.HasConstructorConstraint = $true }
        $CodeTypeParameter | Write-Output;
    }
}

Function Split-CodeTypeReference {
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [ValidateScript({ $_ -is [string] -or $_ -is [System.CodeDom.CodeTypeReference] })]
        [object[]]$TypeReference,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Leaf')]
        [switch]$Leaf,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Container')]
        [switch]$Container
    )

    Process {
        if ($Leaf) {
            foreach ($t in ($TypeReference | ForEach-Object { if ($_ -is [string]) { New-CodeTypeReference -Type $_ } else { $_ } })) {
                    
            }
        } else {
            foreach ($t in ($TypeReference | ForEach-Object { if ($_ -is [string]) { New-CodeTypeReference -Type $_ } else { $_ } })) {
                    
            }
        }
    }
}

Function Test-CodeTypeReference {
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [ValidateScript({ $_ -eq $null -or $_ -is [string] -or $_ -is [System.CodeDom.CodeTypeReference] })]
        [object[]]$TypeReference
    )

    Process {
        if ($null -eq $TypeReference) {
            $false | Write-Output;
        } else {
            foreach ($t in $TypeReference) {
                if ($null -eq $t) {
                    $false | Write-Output;
                } else {
                    <#if ($t -is [string]) {
                        $t | Test-LanguageIndependentIdentifier -AllowNamespace;
                    } else {
                        $t.Namespace | Test-LanguageIndependentIdentifier -AllowNamespace;
                    }#>
                }
            }
        }
    }
}

Function New-CodeTypeReference {
    [OutputType([System.CodeDom.CodeTypeReference])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Type')]
        [ValidateScript({ $_ -is [System.Type] -or ($_ -is [string] -and $_ | Test-CodeTypeReference) })]
        [object[]]$Type,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'TypeParameter')]
        [ValidateScript({ $_ | Test-CodeTypeParameter })]
        [System.CodeDom.CodeTypeParameter[]]$TypeParameter,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Generic')]
        [ValidateScript({ $_ | Test-CodeTypeReference })]
        [string]$Name,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Generic')]
        [System.CodeDom.CodeTypeReference[]]$Arguments,
        
        [Parameter(ParameterSetName = 'Type')]
        [System.CodeDom.CodeTypeReferenceOptions]$Option,
        
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Array')]
        [ValidateScript({ $_ -is [System.Type] -or $_ | Test-CodeTypeReference })]
        [object[]]$ElementType,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Array')]
        [int]$Rank
    )

    Process {
        if ($PSBoundParameters.ContainsKey('Type')) {
            if ($PSBoundParameters.ContainsKey('Option')) {
                $Type | ForEach-Object { New-Object -TypeName 'System.CodeDom.CodeTypeReference' -ArgumentList $_, $Option }
            } else {
                $Type | ForEach-Object { New-Object -TypeName 'System.CodeDom.CodeTypeReference' -ArgumentList $_ }
            }
        } else {
            if ($PSBoundParameters.ContainsKey('TypeParameter')) {
                $TypeParameter | ForEach-Object { New-Object -TypeName 'System.CodeDom.CodeTypeReference' -ArgumentList $_ }
            } else {
                if ($PSBoundParameters.ContainsKey('Arguments')) {
                    New-Object -TypeName 'System.CodeDom.CodeTypeReference' -ArgumentList $Name, (,$Arguments);
                } else {
                    $ElementType | ForEach-Object {
                        if ($_ -is [System.Type]) {
                            New-Object -TypeName 'System.CodeDom.CodeTypeReference' -ArgumentList (New-CodeTypeReference -Type $_), $Rank;
                        } else {
                            New-Object -TypeName 'System.CodeDom.CodeTypeReference' -ArgumentList $_, $Rank;
                        }
                    }
                }
            }
        }
    }
}

Function Find-CodeTypeDeclaration {
    [OutputType([System.CodeDom.CodeTypeDeclaration])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Any')]
        [ValidateScript({ $_ -is [System.CodeDom.CodeCompileUnit] -or $_ -is [System.CodeDom.CodeNamespace] })]
        [object[]]$InputObject,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'CompileUnit')]
        [System.CodeDom.CodeCompileUnit[]]$CompileUnit,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.CodeDom.CodeNamespace[]]$Namespace
    )

    Process {
        $n = '';
        if ($PSBoundParameters.ContainsKey('CompileUnit')) {
            $CompileUnit | ForEach-Object { if ($_.Namespaces.Count -gt 0) { @($_.Namespaces) | ForEach-Object {
            } } }
        } else {
            if ($PSBoundParameters.ContainsKey('Namespace')) {
                $Namespace | ForEach-Object {
                }
            } else {
                $CompileUnit | ForEach-Object { if ($_ -is [System.CodeDom.CodeNamespace]) { $_ } else { if ($_.Namespaces.Count -gt 0) { @($_.Namespaces) } } } | ForEach-Object {
                }
            }
        }
    }
}

Function Add-CodeTypeDeclaration {
    [OutputType([System.CodeDom.CodeTypeDeclaration])]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'CompileUnit')]
        [System.CodeDom.CodeCompileUnit]$CompileUnit,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.CodeDom.CodeNamespace]$Namespace,
        
        [Parameter(Mandatory = $true)]
        [object]$Name
    )

    Process {
        $Namespace | ForEach-Object {
            $ns = $CompileUnit | Find-CodeNamespace -Namespace $_;
            if ($null -eq $ns) {
                New-Object -TypeName 'System.CodeDom.CodeNamespace' -ArgumentList $_;
            } else {
                $ns | Write-Output;
            }
        }
    }
}

$CodeCompileUnit = New-CodeCompileUnit;
$CodeNamespaceImport = ('System', 'System.Collections') | Add-CodeNamespaceImport -CompileUnit $CodeCompileUnit;
$CodeNamespace = Add-CodeNamespace -CompileUnit $CodeCompileUnit -Namespace 'MyNs';

<#
$CodeTypeDeclaration = [System.CodeDom.CodeNamespaceImport]::new('TestClass');
$CodeTypeDeclaration.IsClass = $true;
$CodeTypeParameter = [System.CodeDom.CodeTypeParameter]::new('X');
$CodeTypeReference = [System.CodeDom.CodeTypeReference]::new('class');
$CodeTypeReference.Options = [System.CodeDom.CodeTypeReferenceOptions]::GenericTypeParameter;
$CodeTypeParameter.HasConstructorConstraint = $true;
$CodeTypeParameter.Constraints.Add($CodeTypeReference);
$CodeTypeParameter.Constraints.Add([System.CodeDom.CodeTypeReference]::new('System.IComparable'));
$CodeTypeDeclaration.TypeParameters.Add($CodeTypeParameter);
#>
[System.CodeDom.Compiler.CodeDomProvider]$CodeDomProvider = [System.CodeDom.Compiler.CodeDomProvider]::CreateProvider('c#');
$StringWriter = [System.IO.StringWriter]::new();
[System.CodeDom.Compiler.IndentedTextWriter]$IndentedTextWriter = [System.CodeDom.Compiler.IndentedTextWriter]::new($StringWriter);
$CodeDomProvider.GenerateCodeFromCompileUnit($CodeCompileUnit, $IndentedTextWriter, $null);
$IndentedTextWriter.Flush();
$StringWriter.ToString();