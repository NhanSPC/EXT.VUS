Imports pbs.Helper
Imports System.Data
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.BO.BusinessRules


'Namespace EXT.VUS
<PhoebusCommand(Desc:="VUS Course Type")>
<Serializable()> _
Public Class CourseType
    Inherits Csla.BusinessBase(Of CourseType)
    Implements Interfaces.IGenPartObject
    Implements IComparable
    Implements IDocLink



#Region "Property Changed"
    Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
        MyBase.OnDeserialized(context)
        AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
    End Sub

    Private Sub BO_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        'Select Case e.PropertyName

        '    Case "OrderType"
        '        If Not Me.GetOrderTypeInfo.ManualRef Then
        '            Me._orderNo = POH.AutoReference
        '        End If

        '    Case "OrderDate"
        '        If String.IsNullOrEmpty(Me.OrderPrd) Then Me._orderPrd.Text = Me._orderDate.Date.ToSunPeriod

        '    Case "SuppCode"
        '        For Each line In Lines
        '            line._suppCode = Me.SuppCode
        '        Next

        '    Case "ConvCode"
        '        If String.IsNullOrEmpty(Me.ConvCode) Then
        '            _convRate.Float = 0
        '        Else
        '            Dim conv = pbs.BO.LA.CVInfoList.GetConverter(Me.ConvCode, _orderPrd, String.Empty)
        '            If conv IsNot Nothing Then
        '                _convRate.Float = conv.DefaultRate
        '            End If
        '        End If

        '    Case Else

        'End Select

        pbs.BO.Rules.CalculationRules.Calculator(sender, e)
    End Sub
#End Region

#Region " Business Properties and Methods "
    Private _DTB As String = String.Empty


    Private _coursetypeid As Integer
    <System.ComponentModel.DataObjectField(True, False)> _
    Public ReadOnly Property Coursetypeid() As String
        Get
            Return _coursetypeid
        End Get
    End Property

    Private _coursetypecode As String = String.Empty
    Public Property Coursetypecode() As String
        Get
            Return _coursetypecode
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Coursetypecode", True)
            If value Is Nothing Then value = String.Empty
            If Not _coursetypecode.Equals(value) Then
                _coursetypecode = value
                PropertyHasChanged("Coursetypecode")
            End If
        End Set
    End Property

    Private _idx As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public Property Idx() As String
        Get
            Return _idx.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Idx", True)
            If value Is Nothing Then value = String.Empty
            If Not _idx.Equals(value) Then
                _idx.Text = value
                PropertyHasChanged("Idx")
            End If
        End Set
    End Property

    Private _enabled As Byte
    Public Property Enabled() As Byte
        Get
            Return _enabled
        End Get
        Set(ByVal value As Byte)
            CanWriteProperty("Enabled", True)
            'DELETED_ME
            If Not _enabled.Equals(value) Then
                _enabled = value
                PropertyHasChanged("Enabled")
            End If
        End Set
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

#End Region 'Business Properties and Methods

#Region "Validation Rules"

    Private Sub AddSharedCommonRules()
        'Sample simple custom rule
        'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

        'Sample dependent property. when check one , check the other as well
        'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
    End Sub

    Protected Overrides Sub AddBusinessRules()
        AddSharedCommonRules()

        For Each _field As ClassField In ClassSchema(Of CourseType)._fieldList
            If _field.Required Then
                ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _field.FieldName, 0)
            End If
            If Not String.IsNullOrEmpty(_field.RegexPattern) Then
                ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.RegExMatch, New RegExRuleArgs(_field.FieldName, _field.RegexPattern), 1)
            End If
            '----------using lookup, if no user lookup defined, fallback to predefined by developer----------------------------
            If CATMAPInfoList.ContainsCode(_field) Then
                ValidationRules.AddRule(AddressOf LKUInfoList.ContainsLiveCode, _field.FieldName, 2)
                'Else
                '    Select Case _field.FieldName
                '        Case "LocType"
                '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationType))
                '        Case "Status"
                '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationStatus))
                '    End Select
            End If
        Next
        pbs.BO.Rules.BusinessRules.RegisterBusinessRules(Me)
        MyBase.AddBusinessRules()
    End Sub
#End Region ' Validation

#Region " Factory Methods "

    Private Sub New()
        _DTB = Context.CurrentBECode
    End Sub

    Public Shared Function BlankCourseType() As CourseType
        Return New CourseType
    End Function

    Public Shared Function NewCourseType(ByVal pCoursetypeid As String) As CourseType
        If KeyDuplicated(pCoursetypeid) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("CourseType")))
        Return DataPortal.Create(Of CourseType)(New Criteria(pCoursetypeid.ToInteger))
    End Function

    Public Shared Function NewBO(ByVal ID As String) As CourseType
        Dim pCoursetypeid As String = ID.Trim

        Return NewCourseType(pCoursetypeid)
    End Function

    Public Shared Function GetCourseType(ByVal pCoursetypeid As String) As CourseType
        Return DataPortal.Fetch(Of CourseType)(New Criteria(pCoursetypeid.ToInteger))
    End Function

    Public Shared Function GetBO(ByVal ID As String) As CourseType
        Dim pCoursetypeid As String = ID.Trim

        Return GetCourseType(pCoursetypeid)
    End Function

    Public Shared Sub DeleteCourseType(ByVal pCoursetypeid As String)
        DataPortal.Delete(New Criteria(pCoursetypeid.ToInteger))
    End Sub

    Public Overrides Function Save() As CourseType
        If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
        If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("CourseType")))

        Me.ApplyEdit()
        CourseTypeInfoList.InvalidateCache()
        Return MyBase.Save()
    End Function

    Public Function CloneCourseType(ByVal pCoursetypeid As String) As CourseType

        If CourseType.KeyDuplicated(pCoursetypeid) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

        Dim cloningCourseType As CourseType = MyBase.Clone
        cloningCourseType._coursetypeid = 0

        'Todo:Remember to reset status of the new object here 
        cloningCourseType.MarkNew()
        cloningCourseType.ApplyEdit()

        cloningCourseType.ValidationRules.CheckRules()

        Return cloningCourseType
    End Function

#End Region ' Factory Methods

#Region " Data Access "

    <Serializable()> _
    Private Class Criteria
        Public _coursetypeid As Integer

        Public Sub New(ByVal pCoursetypeid As String)
            _coursetypeid = pCoursetypeid.ToInteger

        End Sub
    End Class

    <RunLocal()> _
    Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
        _coursetypeid = criteria._coursetypeid

        ValidationRules.CheckRules()
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
        Using ctx = ConnectionManager.GetManager
            Using cm = ctx.Connection.CreateCommand()
                cm.CommandType = CommandType.Text
                cm.CommandText = <SqlText>SELECT * FROM CourseType WHERE CourseTypeID= <%= criteria._coursetypeid %></SqlText>.Value.Trim

                Using dr As New SafeDataReader(cm.ExecuteReader)
                    If dr.Read Then
                        FetchObject(dr)
                        MarkOld()
                    End If
                End Using

            End Using
        End Using
    End Sub

    Private Sub FetchObject(ByVal dr As SafeDataReader)
        _coursetypeid = dr.GetInt32("CourseTypeID")
        _coursetypecode = dr.GetString("CourseTypeCode").TrimEnd
        _idx.Text = dr.GetInt32("Idx")
        _enabled = dr.GetString("Enabled").TrimEnd

    End Sub

    Private Shared _lockObj As New Object
    Protected Overrides Sub DataPortal_Insert()
        SyncLock _lockObj
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()

                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "pbs_EXT_VUS_CourseType_InsertUpdate"

                    AddInsertParameters(cm)
                    cm.ExecuteNonQuery()

                End Using
            End Using
        End SyncLock
    End Sub

    Private Sub AddInsertParameters(ByVal cm As SqlCommand)
        cm.Parameters.AddWithValue("@CourseTypeID", _coursetypeid)
        cm.Parameters.AddWithValue("@CourseTypeCode", _coursetypecode.Trim)
        cm.Parameters.AddWithValue("@Idx", _idx.DBValue)
        cm.Parameters.AddWithValue("@Enabled", _enabled)
    End Sub


    Protected Overrides Sub DataPortal_Update()
        DataPortal_Insert()
    End Sub

    Protected Overrides Sub DataPortal_DeleteSelf()
        DataPortal_Delete(New Criteria(_coursetypeid))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
        Using ctx = ConnectionManager.GetManager
            Using cm = ctx.Connection.CreateCommand()

                cm.CommandType = CommandType.Text
                cm.CommandText = <SqlText>DELETE CourseType WHERE CourseTypeID= <%= criteria._coursetypeid %></SqlText>.Value.Trim
                cm.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
        If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
            CourseTypeInfoList.InvalidateCache()
        End If
    End Sub


#End Region 'Data Access                           

#Region " Exists "
    Public Shared Function Exists(ByVal pCoursetypeid As String) As Boolean
        Return CourseTypeInfoList.ContainsCode(pCoursetypeid)
    End Function

    Public Shared Function KeyDuplicated(ByVal pCoursetypeid As String) As Boolean
        Dim SqlText = <SqlText>SELECT COUNT(*) FROM CourseType WHERE CourseTypeID= '<%= pCoursetypeid %>'</SqlText>.Value.Trim
        Return SQLCommander.GetScalarInteger(SqlText) > 0
    End Function
#End Region

#Region " IGenpart "

    Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
        Return CloneCourseType(id)
    End Function

    Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
        Return GetBO(id)
    End Function

    Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
        Return pbs.Helper.Action.StandardReferenceCommands
    End Function

    Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
        Return GetType(CourseType).ToString
    End Function

    Public Function myName() As String Implements Interfaces.IGenPartObject.myName
        Return GetType(CourseType).ToString.Leaf
    End Function

    Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
        Return CourseTypeInfoList.GetCourseTypeInfoList
    End Function
#End Region

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