// src/components/AuthInitializer.tsx
import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { loginSuccess } from "features/auth/authSlice";

export default function AuthInitializer() {
    const dispatch = useDispatch();

    useEffect(() => {
        const userStr = localStorage.getItem("user");
        const user = userStr ? JSON.parse(userStr) : null;
        if (user) {
            dispatch(loginSuccess(user));
        }
    }, [dispatch]);

    return null;
}
