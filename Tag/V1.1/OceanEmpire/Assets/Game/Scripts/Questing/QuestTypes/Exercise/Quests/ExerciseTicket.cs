using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class ExerciseTicket : Quest<ExerciseTicketContext>
    {
        [SerializeField] int aquiredTickets = 0;

        [NonSerialized] bool isListening = false;

        public override string GetDisplayedProgressText()
        {
            return aquiredTickets + "/" + context.ticketAmount;
        }

        public override float GetProgress01()
        {
            return aquiredTickets / (float)context.ticketAmount;
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
            DirtyState = DirtyState.Dirty;
        }

        public override QuestState UpdateState()
        {
            if (state == QuestState.Ongoing)
                ListenIfNotListening();
            return state;
        }

        void ListenIfNotListening()
        {
            if (isListening)
                return;

            PlayerCurrency.TicketChange += PlayerCurrency_TicketChange;
            isListening = true;
        }

        void StopListening()
        {
            PlayerCurrency.TicketChange -= PlayerCurrency_TicketChange;
            isListening = false;
        }

        private void PlayerCurrency_TicketChange(int delta, CurrencyEventArgs eventArgs)
        {
            if (delta > 0)
                aquiredTickets += delta;

            if (aquiredTickets >= context.ticketAmount)
            {
                Complete();
                StopListening();
            }
            else
            {
                DirtyState = DirtyState.UrgentDirty;
            }
        }
    }
}