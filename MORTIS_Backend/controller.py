import mediapipe as mp
import orjson

import config
from server import UDPServer


class FaceDetector:
    def __init__(self, controller, model_path: str):
        self.BaseOptions = mp.tasks.BaseOptions
        self.FaceLandmarker = mp.tasks.vision.FaceLandmarker
        self.FaceLandmarkerOptions = mp.tasks.vision.FaceLandmarkerOptions
        self.FaceLandmarkerResult = mp.tasks.vision.FaceLandmarkerResult
        self.VisionRunningMode = mp.tasks.vision.RunningMode
        self.controller = controller

        options = self.FaceLandmarkerOptions(
            base_options=self.BaseOptions(model_asset_path=model_path),
            running_mode=self.VisionRunningMode.LIVE_STREAM,
            output_face_blendshapes=True,
            num_faces=1,
            result_callback=self.result_callback,
        )

        self.landmarker = self.FaceLandmarker.create_from_options(options)

    def result_callback(self, result, output_image: mp.Image, timestamp_ms: int):
        self.controller.get_result_face(result, timestamp_ms)


class HandDetector:
    def __init__(self, controller, model_path: str):
        self.BaseOptions = mp.tasks.BaseOptions
        self.HandLandmarker = mp.tasks.vision.HandLandmarker
        self.HandLandmarkerOptions = mp.tasks.vision.HandLandmarkerOptions
        self.HandLandmarkerResult = mp.tasks.vision.HandLandmarkerResult
        self.VisionRunningMode = mp.tasks.vision.RunningMode
        self.controller = controller

        options = self.HandLandmarkerOptions(
            base_options=self.BaseOptions(model_asset_path=model_path),
            running_mode=self.VisionRunningMode.LIVE_STREAM,
            min_hand_detection_confidence=0.5,
            min_hand_presence_confidence=0.3,
            min_tracking_confidence=0.3,
            num_hands=2,
            result_callback=self.result_callback,
        )

        self.landmarker = self.HandLandmarker.create_from_options(options)

    def result_callback(self, result, output_image: mp.Image, timestamp_ms: int):
        self.controller.get_result_hands(result, timestamp_ms)


class BodyDetector:
    def __init__(self, controller, model_path: str):
        self.BaseOptions = mp.tasks.BaseOptions
        self.PoseLandmarker = mp.tasks.vision.PoseLandmarker
        self.PoseLandmarkerOptions = mp.tasks.vision.PoseLandmarkerOptions
        self.PoseLandmarkerResult = mp.tasks.vision.PoseLandmarkerResult
        self.VisionRunningMode = mp.tasks.vision.RunningMode
        self.controller = controller

        options = self.PoseLandmarkerOptions(
            base_options=self.BaseOptions(model_asset_path=model_path),
            running_mode=self.VisionRunningMode.LIVE_STREAM,
            result_callback=self.result_callback,
        )

        self.landmarker = self.PoseLandmarker.create_from_options(options)

    def result_callback(self, result, output_image: mp.Image, timestamp_ms: int):
        self.controller.get_result_pose(result, timestamp_ms)


class HandLandmark:
    def __init__(self, handedness):
        self.handedness = handedness
        self.landmarks = list()

    def add_world_landmarks(self, world_landmarks):
        self.landmarks.clear()
        for lm in world_landmarks:
            # 保留5位小数
            self.landmarks.append({"x": round(-lm.x, 5), "y": round(lm.y, 5), "z": round(-lm.z, 5)})

    def as_dict(self):
        return {"handedness": 0 if self.handedness == "left" else 1, "hand_world_landmarks": self.landmarks}


class FaceBlendshape:
    def __init__(self):
        self.result_type = "face_blendshape"
        self.face_blendshape = list()

    def add_face_blendshape(self, face_blendshape):
        self.face_blendshape.clear()
        for bs in face_blendshape:
            self.face_blendshape.append({"name": bs.category_name, "score": bs.score})

    def to_json(self):
        return orjson.dumps({"result_type": self.result_type, "face_blendshape": self.face_blendshape})


class FaceLandmark:
    def __init__(self):
        self.result_type = "face_landmarks"
        self.face_landmarks = list()

    def add_face_landmarks(self, face_landmarks):
        self.face_landmarks.clear()
        for lm in face_landmarks:
            self.face_landmarks.append({"x": round(-lm.x, 5), "y": round(lm.y, 5), "z": round(-lm.z, 5)})

    def to_json(self):
        return orjson.dumps({"result_type": self.result_type, "face_landmarks": self.face_landmarks})


class PoseLandmark:
    def __init__(self):
        self.result_type = "pose"
        self.landmarks = list()

    def add_world_landmarks(self, world_landmarks):
        self.landmarks.clear()
        for lm in world_landmarks:
            self.landmarks.append({"x": round(-lm.x, 5), "y": round(lm.y, 5), "z": round(-lm.z, 5)})

    def to_json(self):
        return orjson.dumps({"result_type": self.result_type, "landmarks": self.landmarks})


class DataController:
    def __init__(self):
        self.hand_detector = HandDetector(self, config.HAND_MODEL)
        self.face_detector = FaceDetector(self, config.FACE_MODEL)
        self.pose_detector = BodyDetector(self, config.POSE_MODEL)
        # 初始化result,用来可视化
        self.face_results = None
        self.hand_results = None
        self.pose_results = None
        self.face_blendshapes_for_visualization = None
        # 初始化UDP服务器
        self.udp_server = UDPServer(config.IP, config.PORT)

    def start(self):
        self.udp_server.start()

    def stop(self):
        self.udp_server.stop_server()

    def detect_async_frame(self, frame, timestamp_ms):
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame)
        self.face_detector.landmarker.detect_async(mp_image, timestamp_ms)
        self.pose_detector.landmarker.detect_async(mp_image, timestamp_ms)
        self.hand_detector.landmarker.detect_async(mp_image, timestamp_ms)

    def get_result_hands(self, result, timestamp_ms):
        self.hand_results = result

        results = []

        for idx, category in enumerate(result.handedness):
            current_hand = HandLandmark(category[0].display_name.lower())
            current_hand.add_world_landmarks(result.hand_world_landmarks[idx])
            results.append(current_hand.as_dict())
        self.udp_server.send_data(orjson.dumps({"hands": results, "result_type": "hands"}))

    def get_result_face(self, result, timestamp_ms):
        self.face_results = result
        self.face_blendshapes_for_visualization = result.face_blendshapes
        if result is not None and len(result.face_blendshapes) > 0:
            self.face_blendshapes = FaceBlendshape()
            self.face_blendshapes.add_face_blendshape(result.face_blendshapes[0])
            self.json_face_blendshapes = self.face_blendshapes.to_json()
            self.udp_server.send_data(self.json_face_blendshapes)

        if result is not None and len(result.face_landmarks) > 0:
            self.face_landmarks = FaceLandmark()
            self.face_landmarks.add_face_landmarks(result.face_landmarks[0])
            self.json_face_landmarks = self.face_landmarks.to_json()
            self.udp_server.send_data(self.json_face_landmarks)

    def get_result_pose(self, result, timestamp_ms):
        self.pose_results = result
        if result is not None and len(result.pose_world_landmarks) > 0:
            self.pose_data = PoseLandmark()
            self.pose_data.add_world_landmarks(result.pose_world_landmarks[0])
            self.json_pose = self.pose_data.to_json()
            self.udp_server.send_data(self.json_pose)
