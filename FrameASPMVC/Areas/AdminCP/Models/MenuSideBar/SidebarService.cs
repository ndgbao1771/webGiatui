using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace App.Menu {

    public class SideBarService
    {
        private readonly IUrlHelper UrlHelper;

        public List<SidebarItem> Items {get; set;} = new List<SidebarItem>();

        public SideBarService(IUrlHelperFactory factory, IActionContextAccessor action)
        {
            UrlHelper = factory.GetUrlHelper(action.ActionContext);
            //khoi tao cac muc sidebar
            Items.Add(new SidebarItem() {Type = SidebarItemType.Divider});
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Thống kê",
                AwesomeIcon = "fas fa-table",
                collapseID = "Statistics",
                Items = new List<SidebarItem>()
                    {
                        new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Statistic",
                            Action = "StatisticOrder",
                            Area = "StatisticReport",
                            Title = "Thống kê kinh doanh"
                        },
                        new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Statistic",
                            Action = "ExportReport",
                            Area = "StatisticReport",
                            Title = "Xuất báo cáo"
                        },

                    }
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem() 
                {
                    Type = SidebarItemType.NavItem,
                    Title = "Quản lí đơn hàng",
                    AwesomeIcon = "fas fa-shopping-basket",
                    collapseID = "Order",
                    Items = new List<SidebarItem>()
                    {
                        new SidebarItem() 
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Order",
                            Action = "Index",
                            Area = "Order",
                            Title = "Danh sách đơn hàng"
                        },
                        
                    }
                });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "WashService",
                Action = "Index",
                Area = "WashService",
                Title = "Quản lí dịch vụ",
                AwesomeIcon = "fas fa-people-carry"
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lí vật liệu",
                AwesomeIcon = "fas fa-pump-soap",
                collapseID = "Ingredient",
                Items = new List<SidebarItem>()
                {
                    new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Ingredient",
                            Action = "Index",
                            Area = "Ingredient",
                            Title = "Danh sách vật liệu"
                        },
                    new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Brand",
                            Action = "Index",
                            Area = "Ingredient",
                            Title = "Danh sách thương hiệu"
                        },
                    new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "CateIngredient",
                            Action = "Index",
                            Area = "Ingredient",
                            Title = "Danh mục loại vật liệu"
                        },
                }
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "ElecEquipment",
                Action = "Index",
                Area = "Ingredient",
                Title = "Quản lí thiết bị",
                AwesomeIcon = "fab fa-uber",
                collapseID = "ElecEquipment",
                Items = new List<SidebarItem>()
                {
                    new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "ElecEquipment",
                            Action = "Index",
                            Area = "Ingredient",
                            Title = "Danh sách máy móc"
                        },
                    new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "CateElecEquipment",
                            Action = "Index",
                            Area = "Ingredient",
                            Title = "Danh mục loại máy móc"
                        }
                }
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Thiết lập tiệm",
                AwesomeIcon = "fas fa-tools",
                collapseID = "ConfigShop",
                Items = new List<SidebarItem>()
                    {
                        new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "ConfigShop",
                            Action = "UnitProduct",
                            Area = "AdminCP",
                            Title = "Thiết lập đơn vị"
                        },
						new SidebarItem()
						{
							Type = SidebarItemType.NavItem,
							Controller = "ConfigShop",
							Action = "SysInfoShop",
							Area = "AdminCP",
							Title = "Thông tin Shop"
						}
				}
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Vai trò & thành viên",
                AwesomeIcon = "fas fa-users-cog",
                collapseID = "Role",
                Items = new List<SidebarItem>()
                    {
                        new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Role",
                            Action = "Index",
                            Area = "Identity",
                            Title = "Quản lí vai trò"
                        },
                        new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Role",
                            Action = "Create",
                            Area = "Identity",
                            Title = "Tạo vai trò",
                        },
                        new SidebarItem()
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "User",
                            Action = "Index",
                            Area = "Identity",
                            Title = "Danh sách thành viên",
                        },
                    }
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "DbManage",
                Action = "Index",
                Area = "Database",
                Title = "Quản lí DB",
                AwesomeIcon = "fas fa-database"
            });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "Contact",
                Action = "Index",
                Area = "Contact",
                Title = "Quản lí phản hồi",
                AwesomeIcon = "fas fa-comment"
            });

        }

        public string RenderHtml()
        {
            var html = new StringBuilder();
            foreach(var item in Items)
            {
                html.Append(item.RenderHtml(UrlHelper)); 
            }
            return html .ToString();
        }

        public void setActive(string Area, string Controller, string Action)
        {
            foreach(var item in Items)
            {
                if(item.Area == Area && item.Controller == Controller && item.Action == Action)
                {
                    item.IsActive = true;
                    return;
                }else
                {
                    if(item.Items != null)
                    {
                        foreach(var childItem in item.Items)
                        {
                            if(childItem.Area == Area && childItem.Controller == Controller && childItem.Action == Action)
                            {
                                childItem.IsActive = true;
                                item.IsActive = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

    }
}