$Text = @'
#region Assembly System.Runtime, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// C:\Program Files\dotnet\sdk\NuGetFallbackFolder\microsoft.netcore.app\2.1.0\ref\netcoreapp2.1\System.Runtime.dll
#endregion

using System.Reflection;

namespace System.Collections.Generic
{
    //
    // Summary:
    //     Represents a collection of objects that can be individually accessed by index.
    //
    // Type parameters:
    //   T:
    //     The type of elements in the list.
    [DefaultMember("Item")]
    public interface IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        //
        // Summary:
        //     Gets or sets the element at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to get or set.
        //
        // Returns:
        //     The element at the specified index.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList`1.
        //
        //   T:System.NotSupportedException:
        //     The property is set and the System.Collections.Generic.IList`1 is read-only.
        T this[int index] { get; set; }

        //
        // Summary:
        //     Determines the index of a specific item in the System.Collections.Generic.IList`1.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.IList`1.
        //
        // Returns:
        //     The index of item if found in the list; otherwise, -1.
        int IndexOf(T item);
        //
        // Summary:
        //     Inserts an item to the System.Collections.Generic.IList`1 at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert into the System.Collections.Generic.IList`1.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList`1.
        //
        //   T:System.NotSupportedException:
        //     The System.Collections.Generic.IList`1 is read-only.
        void Insert(int index, T item);
        //
        // Summary:
        //     Removes the System.Collections.Generic.IList`1 item at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the item to remove.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList`1.
        //
        //   T:System.NotSupportedException:
        //     The System.Collections.Generic.IList`1 is read-only.
        void RemoveAt(int index);
    }
}
'@;

$CommentRegex = [System.Text.RegularExpressions.Regex]::new('//', [System.Text.RegularExpressions.RegexOptions]::Compiled);
$ParamRegex = [System.Text.RegularExpressions.Regex]::new('^(?:   |\t)([^\s\r\n:]+:)+', [System.Text.RegularExpressions.RegexOptions]::Compiled);

$XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
$XmlWriterSettings.Indent = $true;
$XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $false);

[System.Windows.Clipboard]::SetText(([System.Text.RegularExpressions.Regex]::new('[\r\n]+([ \t]+)?//([^/\r\n]*(?:[\r\n]+[ \t]*//[^/\r\n]*)+)[\r\n]+[ \t]*([^\r\n/][^\r\n]*)').Matches($Text) | ForEach-Object {
    $Indent = $_.Groups[1].Value;
    [Xml]$XmlDocument = '<comments />';
    [System.Xml.XmlElement]$XmlElement = $null;
    $Mode = 0;
    $Regex.Split($_.Groups[2].Value) | ForEach-Object { $_.TrimEnd() } | Where-Object { $_.Length -gt 0 } | ForEach-Object {
        $Line = $_;
        switch ($Line) {
            ' Summary:' {
                $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('summary'));
                $XmlElement.IsEmpty = $true;
                $Mode = 0;
                break;
            }
            ' Parameters:' {
                $Mode = 1;
                $XmlElement = $null;
                break;
            }
            ' Returns:' {
                $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('returns'));
                $XmlElement.IsEmpty = $true;
                $Mode = 0;
                break;
            }
            ' Exceptions:' {
                $Mode = 2;
                $XmlElement = $null;
                break;
            }
            ' Type parameters:' {
                $Mode = 3;
                $XmlElement = $null;
            }
            ' Remarks:' {
                $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('remarks'));
                $XmlElement.IsEmpty = $true;
                $Mode = 0;
                break;
            }
            default {
                switch ($Mode) {
                    0 {
                        if ($XmlElement.IsEmpty) { $XmlElement.InnerText = $Line.Trim() } else { $XmlElement.InnerText = $XmlElement.InnerText + ' ' + $Line.Trim() }
                        break;
                    }
                    1 {
                        $m = $ParamRegex.Match($Line);
                        if ($m.Success) {
                            $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('param'));
                            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('name')).Value = $m.Groups[1].Value.Substring(0, $m.Groups[1].Length - 1);
                            $XmlElement.IsEmpty = $true;
                        } else {
                            if ($XmlElement.IsEmpty) { $XmlElement.InnerText = $Line.Trim() } else { $XmlElement.InnerText = $XmlElement.InnerText + ' ' + $Line.Trim() }
                        }
                    }
                    2 {
                        $m = $ParamRegex.Match($Line);
                        if ($m.Success) {
                            $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('exception'));
                            $t = ($m.Groups[1].Value -split ':') | Where-Object { $_.Length -gt 0 } | Select-Object -Last 1;
                            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('cref')).Value = $t;
                            $XmlElement.IsEmpty = $true;
                        } else {
                            if ($XmlElement.IsEmpty) { $XmlElement.InnerText = $Line.Trim() } else { $XmlElement.InnerText = $XmlElement.InnerText + ' ' + $Line.Trim() }
                        }
                    }
                    3 {
                        $m = $ParamRegex.Match($Line);
                        if ($m.Success) {
                            $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('typeparam'));
                            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('name')).Value = $m.Groups[1].Value.Substring(0, $m.Groups[1].Length - 1);
                            $XmlElement.IsEmpty = $true;
                        } else {
                            if ($XmlElement.IsEmpty) { $XmlElement.InnerText = $Line.Trim() } else { $XmlElement.InnerText = $XmlElement.InnerText + ' ' + $Line.Trim() }
                        }
                    }
                }
            }
        }
    }
    "";
    $XmlDocument.DocumentElement.SelectNodes('summary') | ForEach-Object { $_.InnerText = "`n$($_.InnerText)`n" }
    $XmlDocument.DocumentElement.SelectNodes('*') | ForEach-Object { $_.OuterXml -split '\r\n?|\n' } | ForEach-Object { "$Indent///`t$_" }
    "$Indent$($_.Groups[3].Value)";
} | Out-String));