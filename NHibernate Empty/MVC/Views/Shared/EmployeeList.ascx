<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Employee>>" %>
<%@ Import Namespace="NHibernateDemo.Entities.Employees"%>

    <table>
        <tr>
            <th></th>
            <th>
                EmployeeId
            </th>
            <th>
                FirstName
            </th>
            <th>
                LastName
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", "Employee", new { id=item.Id },null) %> |
                <%= Html.ActionLink("Delete", "Delete", "Employee", new { id=item.Id },null) %>
            </td>
            <td>
                <%= Html.Encode(item.Id) %>
            </td>
            <td>
                <%= Html.Encode(item.FirstName) %>
            </td>
            <td>
                <%= Html.Encode(item.LastName) %>
            </td>
        </tr>
    
    <% } %>

    </table>

