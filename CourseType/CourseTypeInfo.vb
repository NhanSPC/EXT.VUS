
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

'Namespace EXT.VUS

<Serializable()> _
Public Class CourseTypeInfo
    Inherits Csla.ReadOnlyBase(Of CourseTypeInfo)
    Implements IComparable
    Implements IInfo
    Implements IDocLink
    'Implements IInfoStatus


#Region " Business Properties and Methods "


    Private _coursetypeid As Integer
    Public ReadOnly Property Coursetypeid() As Integer
        Get
            Return _coursetypeid
        End Get
    End Property

    Private _coursetypecode As String = String.Empty
    Public ReadOnly Property Coursetypecode() As String
        Get
            Return _coursetypecode
        End Get
    End Property

    Private _idx As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public ReadOnly Property Idx() As String
        Get
            Return _idx.Text
        End Get
    End Property

    Private _enabled As Boolean
    Public ReadOnly Property Enabled() As Boolean
        Get
            Return _enabled
        End Get
    End Property

    'Get ID
    Protected Overrides Function GetIdValue() As Object
        Return _coursetypeid
    End Function

    'IComparable
    Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
        Dim ID = IDObject.ToString
        Dim pCoursetypeid As Integer = ID.Trim.ToInteger
        If _coursetypeid < pCoursetypeid Then Return -1
        If _coursetypeid > pCoursetypeid Then Return 1
        Return 0
    End Function

    Public ReadOnly Property Code As String Implements IInfo.Code
        Get
            Return _coursetypeid
        End Get
    End Property

    Public ReadOnly Property LookUp As String Implements IInfo.LookUp
        Get
            Return _coursetypeid
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IInfo.Description
        Get
            Return _coursetypeid
        End Get
    End Property


    Public Sub InvalidateCache() Implements IInfo.InvalidateCache
        CourseTypeInfoList.InvalidateCache()
    End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

    Friend Shared Function GetCourseTypeInfo(ByVal dr As SafeDataReader) As CourseTypeInfo
        Return New CourseTypeInfo(dr)
    End Function

    Friend Shared Function EmptyCourseTypeInfo(Optional ByVal pCoursetypeid As String = "") As CourseTypeInfo
        Dim info As CourseTypeInfo = New CourseTypeInfo
        With info
            ._coursetypeid = pCoursetypeid.ToInteger

        End With
        Return info
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        _coursetypeid = dr.GetInt32("CourseTypeID")
        _coursetypecode = dr.GetString("CourseTypeCode").TrimEnd
        _idx.Text = dr.GetInt16("Idx")
        _enabled = dr.GetBoolean("Enabled")

    End Sub

    Private Sub New()
    End Sub


#End Region ' Factory Methods

#Region "IDoclink"
    Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
        Return String.Format("{0}#{1}", Get_TransType, _coursetypeid)
    End Function

    Public Function Get_TransType() As String Implements IDocLink.Get_TransType
        Return Me.GetType.ToClassSchemaName.Leaf
    End Function
#End Region

End Class

'End Namespace