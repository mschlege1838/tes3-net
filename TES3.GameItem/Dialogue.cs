
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TES3.GameItem.Item;

namespace TES3.GameItem
{
    public class Dialogue : IEnumerable<DialogueResponse>
    {

        public delegate void OnAddResponse(string modFileName, DialogueTopic topic, IList<DialogueResponse> responses);
        public delegate void OnRemoveResponse(string modFileName, DialogueResponse response);

        readonly string modFileName;
        readonly IDictionary<string, DialogueResponse> responses;
        readonly OnAddResponse onAdd;
        readonly OnRemoveResponse onRemove;

        string firstIdentifier;
        string lastIdentifier;

        public Dialogue(string modFileName, DialogueTopic topic, IDictionary<string, DialogueResponse> responses, OnAddResponse onAdd, OnRemoveResponse onRemove)
        {
            this.modFileName = modFileName;
            Topic = topic;
            this.responses = responses;
            this.onAdd = onAdd;
            this.onRemove = onRemove;

            //Console.WriteLine($"{modFileName}/{topic.Name}");
            //foreach (var response in responses.Values)
            //{
            //    Console.WriteLine($"{response.PreviousIdentifier} <- {response.Identifier} -> {response.NextIdentifier}");
            //}
            if (responses.Count > 0)
            {
                firstIdentifier = (from response in responses.Values
                                   where response.PreviousIdentifier.Length == 0 || !responses.ContainsKey(response.PreviousIdentifier)
                                   select response.Identifier).First();
                lastIdentifier = (from response in responses.Values
                                  where response.NextIdentifier.Length == 0 || !responses.ContainsKey(response.NextIdentifier)
                                  select response.Identifier).First();
            }
            else
            {
                firstIdentifier = lastIdentifier = "";
            }
        }

        public DialogueTopic Topic
        {
            get;
        }

        public void AddResponses(IEnumerable<DialogueResponse> responses)
        {
            InsertResponsesAfter(this.responses[lastIdentifier], responses);
        }

        public void InsertResponsesAfter(DialogueResponse target, IEnumerable<DialogueResponse> responses)
        {
            var _responses = new List<DialogueResponse>(responses);
            if (_responses.Count == 0)
            {
                return;
            }

            if (!this.responses.ContainsKey(target.Identifier))
            {
                throw new ArgumentException($"Given target is not part of this dialogue.");
            }


            if (target == null)
            {
                var currentFirst = firstIdentifier == null ? null : this.responses[firstIdentifier];
                
                var enumerator = _responses.GetEnumerator();
                enumerator.MoveNext();

                var current = enumerator.Current;
                this.responses.Add(current.Identifier, current);
                firstIdentifier = current.Identifier;
                while (enumerator.MoveNext())
                {
                    var next = enumerator.Current;
                    this.responses.Add(current.Identifier, current);
                    current.NextIdentifier = next.Identifier;
                    next.PreviousIdentifier = current.Identifier;
                }

                if (currentFirst != null)
                {
                    current.NextIdentifier = currentFirst.Identifier;
                    currentFirst.PreviousIdentifier = current.Identifier;
                }
                else
                {
                    lastIdentifier = current.Identifier;
                }

            }
            else
            {
                var targetNextId = target.NextIdentifier;
                var current = target;
                foreach (var response in _responses)
                {
                    this.responses.Add(response.Identifier, response);
                    response.PreviousIdentifier = current.Identifier;
                    current.NextIdentifier = response.Identifier;
                    current = response;
                }
                if (targetNextId.Length > 0)
                {
                    var targetNext = this.responses[targetNextId];
                    current.NextIdentifier = targetNext.Identifier;
                    targetNext.PreviousIdentifier = current.Identifier;
                }
                else
                {
                    lastIdentifier = current.Identifier;
                }
            }

            onAdd(modFileName, Topic, _responses);
        }

        public void RemoveResponse(DialogueResponse response)
        {
            var prev = responses[response.PreviousIdentifier];
            var next = responses[response.NextIdentifier];

            prev.NextIdentifier = next.Identifier;
            next.PreviousIdentifier = prev.Identifier;
            responses.Remove(response.Identifier);

            onRemove(modFileName, response);
        }

        public IEnumerator<DialogueResponse> GetEnumerator()
        {
            var currentIdentifier = firstIdentifier;
            while (currentIdentifier.Length > 0 && responses.ContainsKey(currentIdentifier))
            {
                var response = responses[currentIdentifier];
                currentIdentifier = response.NextIdentifier;
                yield return response;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}