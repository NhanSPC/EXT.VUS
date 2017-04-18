Imports pbs.Helper
Imports pbs.Helper.Interfaces

Namespace EXT.VUS
    Public Class ScriptInstaller
        Implements IRunable

        Public ReadOnly Property Notes As String Implements IRunable.Notes
            Get
                Return "install db objects"
            End Get
        End Property


        Public Sub Run(args As pbsCmdArgs) Implements IRunable.Run
            'get DBScript from Resources
            'Dim ScriptText = My.Resources.DB_Scripts
            Dim ScriptText = My.Resources.DB_Scripts_EXT_VUS
            Dim xele = XElement.Parse(ScriptText)

            Dim scriplist = New List(Of String)
            'get contents from Install node, then add contents to list
            For Each item In xele...<Install>
                Dim scr = DNz(item.Value, String.Empty)

                If Not String.IsNullOrEmpty(scr) Then scriplist.Add(item.Value)

            Next
            'replace CurrentBECode and excecute contents in the list
            For Each scr In scriplist
                Try
                    Dim decoratedScript = scr.Replace("{XXX}", Context.CurrentBECode)

                    pbs.BO.SQLCommander.RunInsertUpdate(decoratedScript)

                Catch ex As Exception
                End Try
                
            Next

        End Sub
    End Class
End Namespace

