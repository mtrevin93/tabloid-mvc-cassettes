using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult Create(int postId)
        {
            Comment comment = new Comment
            {
                PostId = postId
            };

            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comment comment)
        {
            try
            {
                comment.Author = new UserProfile { Id = GetCurrentUserProfileId() };
                comment.CreateDateTime = DateAndTime.Now;

                _commentRepository.Create(comment);

                return RedirectToAction("Index", new { postId = comment.PostId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Post","Details", new { id = comment.PostId });
            }
        }
        public IActionResult Index(int postId)
        {
            //Replace post from details page with post with comments attached
            Post post = _postRepository.GetPostWithComments(postId);

            var vm = new PostDetailsViewModel
            {
                Post = post,
                CurrentUserId = GetCurrentUserProfileId(),
            };

            return View(vm);
        }

        public IActionResult DeleteComment(Comment comment)
        {
            Post post = _commentRepository.GetPostByComment(comment);

            _commentRepository.Delete(comment);

            return RedirectToAction("Index", new { postId = post.Id });
        }

        public IActionResult Details(int id)
        {
            var currentUserId = GetCurrentUserProfileId();

            var comment = _commentRepository.GetCommentById(id);

            var vm = new CommentDetailsViewModel { Comment = comment, CurrentUserId = currentUserId };

            return View(vm);
        }

        public IActionResult Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }
        [HttpPost]
        public IActionResult Edit(Comment comment)
        {
            //try
            //{
                _commentRepository.Edit(comment);

                return RedirectToAction("Details", new { id = comment.Id });
            //}
            //catch (Exception ex)
            //{
            //    return View(comment);
            //}
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
