<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EditViewModel>" %>
<%@ Import Namespace="NHibernateDemo.MVC.Models"%>
<%@ Import Namespace="NHibernateDemo.Entities"%>
<%@ Import Namespace="NHibernateDemo.DataAccess"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>
        <%  var employee = Model.employee;
            var managers = Model.managers;   
        %>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label>EmployeeId:</label>
                <label><%=  employee.Id %></label>
            </p>
            <p>
                <label for="FirstName">FirstName:</label>
                <%= Html.TextBox("FirstName", employee.FirstName) %>
                <%= Html.ValidationMessage("FirstName", "*") %>
            </p>
            <p>
                <label for="LastName">LastName:</label>
                <%= Html.TextBox("LastName", employee.LastName) %>
                <%= Html.ValidationMessage("LastName", "*") %>
            </p>
            <p>
                <label for="Manager">Manager:</label>
                <%=
                    Html.DropDownList("Manager", managers.Select(p => new SelectListItem()
                                                                       {
                                                                           Text = p.FirstName + " " + p.LastName,
                                                                           Value = p.Id.ToString(),
                                                                           Selected = (p.Id == ((employee.Manager == null) ? 0 : employee.Manager.Id))
                                                                       }))
                %>
            </p>
            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

