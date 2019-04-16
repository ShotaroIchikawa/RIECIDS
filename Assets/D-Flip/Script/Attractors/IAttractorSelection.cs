using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAttractorSelection {
    void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes);
}
