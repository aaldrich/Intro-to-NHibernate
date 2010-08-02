<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Employee>>" %>
<%@ Import Namespace="NHibernateDemo.Entities.Employees"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Employees
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("EmployeeSearch"); %>
    <h2>Employees</h2>

    <% Html.RenderPartial("EmployeeList"); %>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

