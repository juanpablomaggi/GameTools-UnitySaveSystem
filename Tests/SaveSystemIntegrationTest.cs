using NUnit.Framework;

namespace SaveSystem.Test
{
    public class SaveSystemIntegrationTest
    {
        private SaveService saveService;

        [SetUp]
        public void Setup()
        {
            saveService = new SaveService();
            saveService.Init();
        }

        [TearDown]
        public void TearDown()
        {
            saveService.DeleteSave();
            saveService = null;
        }

        [Test]
        public void SaveAndLoad_FullGameState_ShouldRestoreAllObjects()
        {
            var player = new DummyPlayer { Name = "Dummy", Health = 75 };
            var inventory = new DummyInventory();
            inventory.Items.Add("Sword");
            inventory.Items.Add("Potion");

            var quest = new DummyQuest { QuestName = "Find the Relic", IsCompleted = false };

            saveService.RegisterSaveable(player);
            saveService.RegisterSaveable(inventory);
            saveService.RegisterSaveable(quest);

            saveService.SaveGame();

            player.Health = 0;
            player.Name = "";
            inventory.Items.Clear();
            quest.IsCompleted = true;

            saveService.LoadGame();

            Assert.AreEqual("Dummy", player.Name);
            Assert.AreEqual(75, player.Health);
            CollectionAssert.AreEquivalent(new[] { "Sword", "Potion" }, inventory.Items);
            Assert.IsFalse(quest.IsCompleted);
        }
    }
}