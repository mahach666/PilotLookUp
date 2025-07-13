using Ascon.Pilot.SDK;
using System;

namespace PilotLookUp.Interfaces
{
    public interface IMenuService
    {
        void HandleMenuItemClick(string name);
        void HandleToolbarItemClick(string name);
    }
} 