using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSite.Data;
using BlogSite.Models;

namespace BlogSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            int pageCount = 4;
            #region ternary operator stuff
            //int y;
            //if (page == null)
            //{
            //    y = 76;
            //}
            //else
            //{
            //    y = 89;
            //}

            //int y2 = page == null ? 76 : 89;
            //<li class="@(someVar > 100 ? "red" : "green")">
            //page = page == null ? 1 : page;
            #endregion
            page = page ?? 1; //equaivalent to page = page == null ? 1 : page;

            BlogDb db = new BlogDb(Properties.Settings.Default.ConStr);
            HomePageViewModel vm = new HomePageViewModel();
            int total = db.GetPostsCount();
            if (page > 1)
            {
                vm.NextPage = page - 1;
            }
            int from = (page.Value - 1) * pageCount + 1;
            int to = from + (pageCount - 1);
            if (to < total)
            {
                vm.PreviousPage = page + 1;
            }
            vm.Posts = db.GetPosts(from, to);
            return View(vm);
        }

        public ActionResult ViewBlog(int? id)
        {
            if (!id.HasValue)
            {
                return Redirect("/"); //if no id was sent in, redirect to home page
            }
            BlogDb db = new BlogDb(Properties.Settings.Default.ConStr);
            ViewBlogViewModel vm = new ViewBlogViewModel();
            vm.Post = db.GetPost(id.Value);
            vm.Comments = db.GetComments(id.Value);
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            BlogDb db = new BlogDb(Properties.Settings.Default.ConStr);
            comment.DateCreated = DateTime.Now;
            db.AddComment(comment);
            return Redirect($"/home/viewblog?id={comment.PostId}");
        }

        public ActionResult MostRecent()
        {
            BlogDb db = new BlogDb(Properties.Settings.Default.ConStr);
            int id = db.GetMostRecentId();
            return Redirect($"/home/viewblog?id={id}");
        }


    }
}