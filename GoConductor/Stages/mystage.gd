extends BaseStage


@onready var frog_theme = $TrackSwitch/vr
@onready var vr_theme = $TrackSwitch/frog


func frog_chase(b: bool):
    frog_theme.CueIdx(1)


func vr_chase(b: bool):
    vr_theme.CueIdx(1)