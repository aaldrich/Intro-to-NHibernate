<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    using (Html.BeginForm("Search","Employee"))
    {
        %>First Name: <%= Html.TextBox("firstName")
        %><br /> Last Name: <%= Html.TextBox("lastName")%>
        
       <input type="submit" value="Search"/>                    
    <%}
%>