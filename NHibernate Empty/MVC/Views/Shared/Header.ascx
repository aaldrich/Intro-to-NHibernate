<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Rendering.HeaderRenderControl>" %>   
<%@ Import Namespace="Rendering" %>


<div class="header" >
    <% HeaderRenderControl details = this.ViewData.Model; %>

    <div class="siteId" title="<%=details.Logo.Title%>" >
        <a href="<%=details.Logo.Url%>" ><img src="<%=this.Url.Action("layout", "content")%>/site_id.jpg" alt="<%=details.Logo.AlternateText %>" /></a>
    </div>
    
    <div class="tabs" >
    <% foreach (TabDetails tab in details.Tabs) { %><span title="<%=tab.Title%>"><a href="<%=tab.Url%>" ><img src="<%=details.GetTabUrl(tab)%>" alt="<%=tab.AlternateText%>" /></a></span><% } %>
    </div>
    
</div>