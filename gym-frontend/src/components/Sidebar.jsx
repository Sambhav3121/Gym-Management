import { NavLink, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { getProfile } from "../services/api";

const Item = ({ to, icon, children }) => (
  <NavLink
    to={to}
    className={({ isActive }) =>
      `flex items-center gap-3 px-5 py-3 rounded-xl text-gray-700 hover:text-orange-600 hover:bg-orange-50
       ${isActive ? "bg-orange-600 text-white hover:text-white hover:bg-orange-600" : ""}`
    }
  >
    <span className="text-xl">{icon}</span>
    <span className="font-medium">{children}</span>
  </NavLink>
);

export default function Sidebar() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    getProfile()
      .then((r) => setUser(r.data))
      .catch(() => setUser(null));
  }, []);

  const firstName =
    user?.fullName?.trim()?.split(" ")?.[0] ?? "Member";

  return (
    <aside className="h-screen sticky top-0 w-72 border-r bg-white flex flex-col">
      {/* Brand */}
      <div className="px-6 py-5 border-b">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-2xl bg-orange-600 text-white flex items-center justify-center text-2xl">💪</div>
          <div className="text-2xl font-extrabold">SPORTIFY</div>
        </div>
      </div>

      {/* Menu */}
      <nav className="p-4 flex-1 flex flex-col gap-2">
        <Item to="/dashboard" icon="🧩">Dashboard</Item>
        <Item to="/profile" icon="👤">Profile</Item>
        <Item to="/membership" icon="🏅">Membership</Item>
        <Item to="/classes" icon="📅">Classes</Item>
        <Item to="/workouts" icon="🧱">Workout Plans</Item>
        <Item to="/payments" icon="💳">Payments</Item>
        <Item to="/feedback" icon="💬">Feedback</Item>
      </nav>

      {/* Footer */}
      <div className="mt-auto p-4 border-t">
        <div className="px-2 mb-3">
          <div className="text-gray-900 font-semibold">{firstName}</div>
          <div className="text-gray-500 text-sm truncate">{user?.email ?? ""}</div>
        </div>
        <button
          onClick={() => {
            localStorage.removeItem("token");
            navigate("/login");
          }}
          className="w-full flex items-center justify-center gap-2 bg-yellow-500 hover:bg-yellow-600 text-white font-semibold py-2.5 rounded-2xl transition"
        >
          <span className="text-lg">↪</span> Logout
        </button>
      </div>
    </aside>
  );
}
