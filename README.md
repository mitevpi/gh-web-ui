# Grasshopper Web UI

![GitHub issues](https://img.shields.io/github/issues/mitevpi/gh-web-ui)
![GitHub contributors](https://img.shields.io/github/contributors/mitevpi/gh-web-ui)
![GitHub](https://img.shields.io/github/license/mitevpi/gh-web-ui)
[![Generic badge](https://img.shields.io/badge/Demo-YouTube-Green.svg)](https://youtu.be/S-c7z2ezoZo)

Prototype for building a Grasshopper interface using native web components.

# Development

1. Clone locally
2. Re-link and restore dependencies
3. Build (everything shoudl copy to the Grasshopper/Libraries location)
4. Open the [`ghtest.gh`](./grasshopper/ghtest.gh)

## Usage

#### Grasshopper & HTML

After building the `.gha` from source, open the [Grasshopper File](grasshopper/ghtest.gh) for testing.

|                                      Links                                      |                               Sample                                |
| :-----------------------------------------------------------------------------: | :-----------------------------------------------------------------: |
|      [Vue.js UI](GHUI/Web%20UI/InputVue.html)      |     ![Project Plans By Name](assets/images/vue-ui.png)     |
| [Bootstramp HTML UI](GHUI/Web%20UI/InputBootstrap.html) | ![Project Plans By Name](assets/images/bootstrap-ui.png) |


## Tentative Roadmap

### Abstract

At this stage, I'm not sure where this wants to go. All I know is that Human UI from Andrew Heumann has been
an incredibly powerful force in the industry over the last X years - it helps us share and democratize
computational know-how and deliver it to our colleagues who may not be as techincally inclined. I think it's
time for the next step in this journey. With the web becoming the standard for app development, interaction design,
and so much more - I think it makes sense to use this as the backbone for the project.

### Roadmap

- Ensure a Chromium-based execution environment.
- Read values from the DOM.
- Add elements/outputs to the DOM.
- Create components to build an interface/DOM using Grasshopper only (no web dev knowledge).

### Credits

Enormous thanks to everyone who has contributed in any way! I've tried to include everyone here, but I may have missed someone.
If that's the case, I apologize :) - please reach out anytime.

- [Deyan Nenov](https://www.linkedin.com/posts/petr-mitev_chromium-rhino-grasshopper-activity-6779450476074205184-J6Ek)
- [Christopher Connock](https://twitter.com/ChrisConnock/status/1374050893742669824)
- [Ehsan Iran-Nejad](https://www.linkedin.com/in/eirannejad/)
- [Andrew Heumann (Obviously)](https://twitter.com/andrewheumann)
- Andrea Botti
