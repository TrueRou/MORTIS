import time

import cv2

from controller import DataController
from utils import draw_facelandmarks_on_image, draw_handlandmarks_on_image, draw_poselandmarks_on_image

if __name__ == "__main__":
    controller = DataController()
    controller.start()  # 启动UDP服务器

    counter = 0

    cap = cv2.VideoCapture(3)
    while cap.isOpened():
        loop_start_time = time.time()
        ret, frame = cap.read()
        if not ret:
            break
        timestamp_ms = int(cap.get(cv2.CAP_PROP_POS_MSEC))
        controller.detect_async_frame(frame, timestamp_ms)

        if controller.face_results is not None:
            frame = draw_facelandmarks_on_image(frame, controller.face_results)
        if controller.pose_results is not None:
            frame = draw_poselandmarks_on_image(frame, controller.pose_results)
        if controller.hand_results is not None:
            frame = draw_handlandmarks_on_image(frame, controller.hand_results)

        cv2.namedWindow("frame", cv2.WINDOW_NORMAL)
        cv2.moveWindow("frame", 1000, 100)
        cv2.imshow("frame", frame)

        counter += 1
        elapsed_time = time.time() - loop_start_time
        # print(f"第 {counter} 次循环执行检测时间: {elapsed_time:.5f} 秒")

        if cv2.waitKey(1) & 0xFF == 27:
            cap.release()
            controller.stop()
            cv2.destroyAllWindows()
            break
