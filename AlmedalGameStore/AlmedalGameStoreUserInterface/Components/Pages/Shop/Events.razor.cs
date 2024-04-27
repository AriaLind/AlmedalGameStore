using AlmedalGameStoreShared.Dtos.Event;
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreUserInterface.Services;
using Microsoft.AspNetCore.Components;

namespace AlmedalGameStoreUserInterface.Components.Pages.Shop
{
    public partial class Events
    {
        [Inject] private EventService eventService { get; set; }
        [Inject] private ActiveShoppingCartService activeShoppingCartService { get; set; }

        public List<EventDto> allEvents { get; set; } = new();

        public bool AddedToCartMessageVisible { get; set; } = false;

        public bool AddedToCartModalMessageVisible { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            var events = await eventService.GetAllAsync();

            allEvents = events.ToList();
        }

        public async Task AddToCart(ProductDto product)
        {
            AddedToCartMessageVisible = true;
            await activeShoppingCartService.AddItemToCart(product);
            await ToggleAddedToCartModal();
        }

        public async Task AddToCartModal(ProductDto product)
        {
            AddedToCartModalMessageVisible = true;
            await activeShoppingCartService.AddItemToCart(product);
            await ToggleAddedToCartModal();
        }


        private bool showModal = false;
        private ProductDto chosenProduct = new();

        private async Task ShowModal(ProductDto prod)
        {
            showModal = true;
            chosenProduct = prod;

        }

        private async Task CloseModal()
        {
            showModal = false;
        }

        bool addedToCartModal = false;

        Task ToggleAddedToCartModal()
        {
            addedToCartModal = !addedToCartModal;

            return Task.CompletedTask;
        }
    }
}
