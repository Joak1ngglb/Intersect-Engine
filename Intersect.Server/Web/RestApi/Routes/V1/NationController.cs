using Intersect.GameObjects;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Entities;
using Intersect.Server.Localization;
using Intersect.Server.Web.RestApi.Payloads;
using Intersect.Server.Web.RestApi.Types;
using Intersect.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Intersect.Server.Web.RestApi.Routes.V1
{
    [Route("api/v1/nations")]
    [Authorize]
    public sealed partial class NationController : IntersectController
    {
        [HttpPost]
        public object ListPost([FromBody] PagingInfo pageInfo)
        {
            pageInfo.Page = Math.Max(pageInfo.Page, 0);
            pageInfo.Count = Math.Max(Math.Min(pageInfo.Count, 100), 5);
            var entries = Nation.List(null, null, SortDirection.Ascending, pageInfo.Page * pageInfo.Count, pageInfo.Count, out int entryTotal);
            return new
            {
                total = entryTotal,
                pageInfo.Page,
                count = entries.Count,
                entries
            };
        }
        [HttpGet]
        public DataPage<KeyValuePair<Nation, int>> List(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 0,
            [FromQuery] int limit = PAGE_SIZE_MAX,
            [FromQuery] string sortBy = null,
            [FromQuery] SortDirection sortDirection = SortDirection.Ascending,
            [FromQuery] string search = null
        )
        {
            page = Math.Max(page, 0);
            pageSize = Math.Max(Math.Min(pageSize, 100), 5);
            limit = Math.Max(Math.Min(limit, pageSize), 1);
            var values = Nation.List(search?.Length > 2 ? search : null, sortBy, sortDirection, page * pageSize, pageSize, out int total);
            if (limit != pageSize)
            {
                values = values.Take(limit).ToList();
            }
            return new DataPage<KeyValuePair<Nation, int>>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Count = values.Count,
                Values = values
            };
        }
        [HttpGet("{nationId:guid}")]
        public object NationGet(Guid nationId)
        {
            var nation = Nation.LoadNation(nationId);
            if (nation == null)
            {
                return BadRequest($@"Nation not found: ${nationId}.");
            }
            return nation;
        }
        [HttpPost("{nationId:guid}/name")]
        public object ChangeName(Guid nationId, [FromBody] NameChange change)
        {
            if (!FieldChecking.IsValidNationName(change.Name, Strings.Regex.NationName))
            {
                return BadRequest($@"Invalid nation name.");
            }
            var nation = Nation.LoadNation(nationId);
            if (nation == null)
            {
                return NotFound($@"No nation with id '{nationId}'.");
            }
            if (nation.Rename(change.Name))
            {
                return nation;
            }
            return BadRequest($@"Invalid name, or name already taken.");
        }
        [HttpGet("{nationId:guid}/members")]
        public DataPage<Player> Members(
            Guid nationId,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 0,
            [FromQuery] int limit = PAGE_SIZE_MAX,
            [FromQuery] string sortBy = null,
            [FromQuery] SortDirection sortDirection = SortDirection.Ascending,
            [FromQuery] string search = null
        )
        {
            page = Math.Max(page, 0);
            pageSize = Math.Max(Math.Min(pageSize, 100), 5);
            limit = Math.Max(Math.Min(limit, pageSize), 1);
            var values = Player.List(search?.Length > 2 ? search : null, sortBy, sortDirection, page * pageSize, pageSize, out int total, nationId);
            if (limit != pageSize)
            {
                values = values.Take(limit).ToList();
            }
            return new DataPage<Player>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Count = values.Count,
                Values = values
            };
        }
    }
}