﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Client.Providers;
using Website.Shared.Models.Database;

namespace Website.Client.Pages.User.MessagesPage
{
    [Authorize]
    public partial class MessagesPage
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthState { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        private SteamAuthProvider SteamAuth => AuthState as SteamAuthProvider;

        public List<MMessage> Messages { get; set; }

        public IEnumerable<MMessage> ActiveMessages => Messages.Where(x => !x.IsClosed);
        public IEnumerable<MMessage> ClosedMessages => Messages.Where(x => x.IsClosed);

        protected override async Task OnInitializedAsync()
        {
            Messages = await HttpClient.GetFromJsonAsync<List<MMessage>>("api/messages");
            await MessageService.RefreshMessages(Messages); // Don't know if this will block the page from loading, if it does move this into a Task.Run() somewhere
        }

        private void GoToMessage(MMessage msg)
        {
            NavigationManager.NavigateTo("/messages/" + msg.Id);
        }
    }
}
