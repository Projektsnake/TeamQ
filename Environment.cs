﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

class Environment {
	private List<Entity> m_entities = new List<Entity>();
	private Controller m_controller;

	public Environment(Controller ctrl) {
		m_controller = ctrl;
	}

	public ContentManager contentManager {
		get {
			return m_controller.contentManager;
		}
	}

	/*************************************************************************/
	// Game loop.

	public void Update(float elapsedTime) {
		foreach (Entity ent in m_entities) {
			ent.Update(elapsedTime);
		}
	}

	public void Draw(SpriteBatch spriteBatch) {
		foreach (Entity ent in m_entities) {
			ent.Draw(spriteBatch);
		}
	}

	/*************************************************************************/
	// For use internally by Entities.

	public void RegisterEntity(Entity ent) {
		m_entities.Add(ent);
	}

	public void UnregisterEntity(Entity ent) {
		m_entities.Remove(ent);
	}
}