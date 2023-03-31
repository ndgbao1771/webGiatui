using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;
using App.Models.Orders;
using App.Data;

namespace App.ExtendMethods
{
    public static class StateToVN
    {
        public static string StateToVNs(this State ToVN)
        {
            switch(ToVN){
                case State.Process:
                return "<p class='text-info'>Đang xử lí</p>";

                case State.Confirm:
                return "<p class='text-primary'>Đang tiến hành</p>";

                case State.Finished:
                return "<p class='text-success'>Hoàn thành</p>";

                case State.Paid:
                    return "<p class='text-success'>Đã thanh toán</p>";

                case State.ACancel:
                return "<p class='text-danger'>Đơn hàng bị hủy bởi ADMIN</p>";

                case State.CCancel:
                return "<p class='text-danger'>Đơn hàng bị hủy bởi USER</p>";

                default: return "";
            }
             


            
        } 
    }
}