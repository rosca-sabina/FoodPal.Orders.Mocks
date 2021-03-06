﻿using FoodPal.Orders.Mock.MessageBroker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodPal.Orders.Mock.BackgroundServices.Workers
{
    public class PrestoPizzaWorker : BaseWorker, IHostedService
	{
		private const string ProviderName = "prestopizza";

		private readonly ILogger<PrestoPizzaWorker> _logger;

		public PrestoPizzaWorker(ILogger<PrestoPizzaWorker> logger, IMessageBroker messageBroker) : base(messageBroker)
		{
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"{this.GetType().Name} starting; registering message handler.");
			MessageBroker.RegisterMessageReceiver($"provider-{ProviderName}-request", ProcessMessageAsync);
			await MessageBroker.StartListenerAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"{this.GetType().Name} stopping.");
			await MessageBroker.StopListenerAsync();
		}

		private async Task ProcessMessageAsync(string messageEnvelopeAsString)
		{
			// order is being processed...
			var rnd = new Random();
			var secondsRequiredForProcessing = rnd.Next(5, 10); // between 5 and 10 seconds

			await ProcessMessageAsync(messageEnvelopeAsString, ProviderName, secondsRequiredForProcessing);
		}
	}
}
