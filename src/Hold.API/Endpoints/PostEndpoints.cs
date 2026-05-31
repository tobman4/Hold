using Hold.API.Data;
using Hold.API.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;

namespace Hold.API.Endpoints;

public static class PostEndpoints {
  public static T MapPostEndpoints<T>(this T app) where T : IEndpointRouteBuilder {
    app.MapGet("/", GetPosts);
    app.MapPost("/", CreatePost);
    return app;
  }

  private static async Task<IResult> GetPosts(HoldDbContext db) {
    var posts = await db.Posts.OrderByDescending(p => p.Time).ToListAsync();
    return TypedResults.Ok(posts);
  }

  private static async Task<IResult> CreatePost(Post post, HoldDbContext db) {
    db.Posts.Add(post);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/posts/{post.ID}", post);
  }
}
