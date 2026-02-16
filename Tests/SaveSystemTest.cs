using NUnit.Framework;

namespace SaveSystem.Test
{
    public class SaveSystemTest
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
            saveService.DeleteSave(0);
            saveService.DeleteSave(1);
            saveService = null;
        }

        [Test]
        public void Register_ShouldAddSaveable()
        {
            var player = new DummyPlayer { Name = "DummyPlayer"};
            saveService.RegisterSaveable(player);

            saveService.SaveGame();
            saveService.LoadGame();

            Assert.AreEqual("DummyPlayer", player.Name);
        }

        [Test]
        public void SaveAndLoad_ShouldPersistState()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame();

            player.Name = "DummyPlayer2";

            saveService.LoadGame();

            Assert.AreEqual("DummyPlayer", player.Name);
        }

        [Test]
        public void SaveAndSaveAgain_ShouldOverride()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame();

            player.Name = "DummyPlayer2";

            saveService.SaveGame(1);

            saveService.LoadGame(1);

            Assert.AreEqual("DummyPlayer2", player.Name);
        }

        [Test]
        public void Load_ShouldRestoreDeferredObjects()
        {
            var player = new DummyPlayer { Name = "DummyPlayer" };
            saveService.RegisterSaveable(player);
            saveService.SaveGame();

            saveService = new SaveService();
            saveService.Init();
            saveService.LoadGame();

            var newPlayer = new DummyPlayer();
            saveService.RegisterSaveable(newPlayer);

            Assert.AreEqual("DummyPlayer", newPlayer.Name);
        }

        [Test]
        public void Unregister_ShouldRemoveSaveable()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);
            saveService.UnregisterSaveable(player.SaveKey);

            saveService.SaveGame();
            player.Name = "DummyPlayer";

            saveService.LoadGame();

            Assert.AreEqual("DummyPlayer", player.Name);
        }

        [Test]
        public void SaveOnDifferentSaveSlot_ShouldUpdateCurrentSlot()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame(0);

            player.Name = "DummyPlayerOnSecondSlot";

            saveService.SaveGame(1);

            Assert.AreEqual(1, saveService.CurrentSaveSlot);
        }

        [Test]
        public void SaveAndLoadDifferentSaveSlots_ShouldPersistState()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame(0);

            player.Name = "DummyPlayerOnSecondSlot";

            saveService.SaveGame(1);

            saveService.LoadGame(0);

            Assert.AreEqual("DummyPlayer", player.Name);
        }

        [Test]
        public void SaveAndLoadDifferentSaveSlotsChanging_ShouldPersistState()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame();

            player.Name = "DummyPlayerOnSecondSlot";

            saveService.SaveGame(1);

            saveService.LoadGame(1);

            Assert.AreEqual("DummyPlayerOnSecondSlot", player.Name);
        }

        [Test]
        public void SaveAndLoadDifferentSaveSlotsChangingToCurrent_ShouldPersistState()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame();

            player.Name = "DummyPlayerOnSecondSlot";

            saveService.SaveGame(1);

            saveService.LoadGame();

            Assert.AreEqual("DummyPlayerOnSecondSlot", player.Name);
        }

        [Test]
        public void DeleteSave_ShouldDeleteSave()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame(0);

            saveService.DeleteSave(0);

            var load = saveService.LoadGame(0);

            Assert.IsFalse(load, "No file should be available");
        }

        [Test]
        public void DeleteCurrentSave_ShouldDeleteSave()
        {
            var player = new DummyPlayer();
            saveService.RegisterSaveable(player);

            player.Name = "DummyPlayer";

            saveService.SaveGame();

            saveService.DeleteSave();

            var load = saveService.LoadGame();

            Assert.IsFalse(load, "No file should be available");
        }
    }
}