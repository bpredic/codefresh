using Contracts;
using Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
	public class OwnerRepositoryTests
	{
		[Fact]
		public void GetAllOwners_ReturnsListOfOwners_WithSingleOwner()
		{
			// Arrange
			var mockRepo = new Mock<IOwnerRepository>();
			mockRepo.Setup(repo => (repo.GetAllOwners())).Returns(GetOwners());

			// Act
			var result = mockRepo.Object.GetAllOwners().ToList();

			// Assert
			Assert.IsType<List<Owner>>(result);
			Assert.Single(result);
		}

		public List<Owner> GetOwners()
		{
			return new List<Owner>
			{
				new Owner
				{
					Id = Guid.NewGuid(),
					Name = "John Keen",
					DateOfBirth = new DateTime(1980, 12, 05),
					Address = "61 Wellfield Road"
				}
			};
		}

		[Fact]
		public async Task GetAllOwners_ReturnsOkResponse()
		{
			// Act
			var response = await _context.Client.GetAsync("/api/owner");
			response.EnsureSuccessStatusCode();
			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
		[Fact]
		public async Task GetAllOwners_ReturnsAListOfOwners()
		{
			// Act
			var response = await _context.Client.GetAsync("/api/owner");
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var owners = JsonConvert.DeserializeObject<List<OwnerDto>>(responseString);
			// Assert
			Assert.NotEmpty(owners);
		}
	}
}
