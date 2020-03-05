using TES3.Records;

namespace TES3.GameItem.Item
{
    public abstract class ServicesProvider : TES3GameItem
    {
        public ServicesProvider(string id) : base(id)
        {

        }

        public ServicesProvider(Record record) : base(record)
        {

        }

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public bool BuysWeapons
        {
            get;
            set;
        }

        public bool BuysArmor
        {
            get;
            set;
        }

        public bool BuysClothing
        {
            get;
            set;
        }

        public bool BuysBooks
        {
            get;
            set;
        }

        public bool BuysIngredients
        {
            get;
            set;
        }

        public bool BuysPicks
        {
            get;
            set;
        }

        public bool BuysProbes
        {
            get;
            set;
        }

        public bool BuysLights
        {
            get;
            set;
        }

        public bool BuysApparatus
        {
            get;
            set;
        }

        public bool OffersRepair
        {
            get;
            set;
        }

        public bool BuysMiscItems
        {
            get;
            set;
        }

        public bool SellsSpells
        {
            get;
            set;
        }

        public bool BuysMagicItems
        {
            get;
            set;
        }

        public bool BuysPotions
        {
            get;
            set;
        }

        public bool OffersTraining
        {
            get;
            set;
        }

        public bool OffersSpellmaking
        {
            get;
            set;
        }

        public bool OffersEnchanting
        {
            get;
            set;
        }

        public bool BuysRepairItems
        {
            get;
            set;
        }

        public bool IsMerchant
        {
            get => BuysApparatus || BuysArmor || BuysBooks || BuysClothing || BuysIngredients
                        || BuysLights || BuysMagicItems || BuysMiscItems || BuysPicks || BuysPotions || BuysProbes
                        || BuysRepairItems || OffersTraining || BuysWeapons;
        }

        public bool OffersServices
        {
            get => OffersEnchanting || OffersRepair || SellsSpells || OffersSpellmaking || OffersTraining;
        }


        protected void CopyClone(ServicesProvider clone)
        {
            clone.BuysWeapons = BuysWeapons;
            clone.BuysArmor = BuysArmor;
            clone.BuysClothing = BuysClothing;
            clone.BuysBooks = BuysBooks;
            clone.BuysIngredients = BuysIngredients;
            clone.BuysPicks = BuysPicks;
            clone.BuysProbes = BuysProbes;
            clone.BuysLights = BuysLights;
            clone.BuysApparatus = BuysApparatus;
            clone.OffersRepair = OffersRepair;
            clone.BuysMiscItems = BuysMiscItems;
            clone.SellsSpells = SellsSpells;
            clone.BuysMagicItems = BuysMagicItems;
            clone.BuysPotions = BuysPotions;
            clone.OffersTraining = OffersTraining;
            clone.OffersSpellmaking = OffersSpellmaking;
            clone.OffersEnchanting = OffersEnchanting;
            clone.BuysRepairItems = BuysRepairItems;
        }

    }
}
