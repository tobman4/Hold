using Hold.API.Data;
using Hold.API.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;

namespace Hold.API.Endpoints;

public static class HoldItemEndpoints {
  public static WebApplication MapHoldItemEndpoints(this WebApplication app) {
    var group = app.MapGroup("/hold");

    group.MapGet("/", GetHoldItems);
    group.MapGet("/{id:guid}", GetHoldItem);
    group.MapPost("/", CreateHoldItem);
    group.MapPut("/{id:guid}", UpdateHoldItem);
    group.MapDelete("/{id:guid}", DeleteHoldItem);

    return app;
  }

  private static async Task<IResult> GetHoldItems(HoldDbContext db) {
    var items = await db.HoldItems.ToListAsync();
    return TypedResults.Ok(items);
  }

  private static async Task<IResult> GetHoldItem(Guid id, HoldDbContext db) {
    var item = await db.HoldItems.FindAsync(id);

    if (item is null) {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(item);
  }

  private static async Task<IResult> CreateHoldItem(HoldItem holdItem, HoldDbContext db) {
    db.HoldItems.Add(holdItem);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/hold/{holdItem.ID}", holdItem);
  }

  private static async Task<IResult> UpdateHoldItem(Guid id, HoldItem inputItem, HoldDbContext db) {
    var item = await db.HoldItems.FindAsync(id);

    if (item is null) {
      return TypedResults.NotFound();
    }

    item.Name = inputItem.Name;
    item.Description = inputItem.Description;
    item.Count = inputItem.Count;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
  }

  private static async Task<IResult> DeleteHoldItem(Guid id, HoldDbContext db) {
    if (await db.HoldItems.FindAsync(id) is HoldItem item) {
      db.HoldItems.Remove(item);
      await db.SaveChangesAsync();
      return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
  }
}
