import { useEffect, useState } from "react";
import { getAttendanceRecent, getProfile } from "../services/api";

function Row({ label, value }) {
  return (
    <div>
      <div className="text-gray-600 text-sm">{label}</div>
      <div className="text-lg font-semibold">{value ?? "—"}</div>
    </div>
  );
}

export default function Profile() {
  const [user, setUser] = useState(null);
  const [recent, setRecent] = useState([]);

  const [weight, setWeight] = useState(""); // kg
  const [height, setHeight] = useState(""); // cm
  const bmi =
    Number(weight) > 0 && Number(height) > 0
      ? (Number(weight) / Math.pow(Number(height) / 100, 2)).toFixed(1)
      : null;

  useEffect(() => {
    getProfile().then((r) => setUser(r.data)).catch(() => setUser(null));
    getAttendanceRecent(20).then((r) => setRecent(r.data || [])).catch(() => setRecent([]));
  }, []);

  const firstName = user?.fullName?.split(" ")?.[0] ?? "Member";

  const fmtDate = (d) =>
    new Date(d).toLocaleDateString(undefined, {
      year: "numeric",
      month: "numeric",
      day: "numeric",
    });

  return (
    <div className="flex-1 overflow-y-auto">
      <div className="max-w-7xl mx-auto px-8 py-8">
        <h1 className="text-3xl font-extrabold mb-2">My Profile</h1>
        <p className="text-gray-500 mb-6">
          Manage your personal information and track your fitness
        </p>

        <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
          {/* Personal Info */}
          <div className="rounded-2xl border bg-white p-6 shadow-sm">
            <div className="text-lg font-semibold mb-1">Personal Information</div>
            <div className="text-gray-500 text-sm mb-5">Your account details</div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
              <Row label="Full Name" value={user?.fullName} />
              <Row label="Email" value={user?.email} />
              <Row label="Phone" value={user?.phoneNumber || "—"} />
              <Row label="Member Since" value={user?.createdAt ? fmtDate(user.createdAt) : "—"} />
            </div>

            <div className="mt-6">
              <a
                href="/edit-profile"
                className="inline-flex items-center px-4 py-2 rounded-xl bg-yellow-500 hover:bg-yellow-600 text-white font-semibold"
              >
                Edit Profile
              </a>
            </div>
          </div>

          {/* BMI */}
          <div className="rounded-2xl border bg-white p-6 shadow-sm">
            <div className="text-lg font-semibold mb-1 flex items-center gap-2">
              BMI Calculator
            </div>
            <div className="text-gray-500 text-sm mb-5">
              Calculate your Body Mass Index
            </div>

            <div className="space-y-4">
              <div>
                <div className="text-sm text-gray-600 mb-1">Weight (kg)</div>
                <input
                  value={weight}
                  onChange={(e) => setWeight(e.target.value)}
                  className="w-full border rounded-xl px-3 py-2 outline-none focus:ring-2 focus:ring-yellow-400"
                  placeholder="Enter weight"
                />
              </div>
              <div>
                <div className="text-sm text-gray-600 mb-1">Height (cm)</div>
                <input
                  value={height}
                  onChange={(e) => setHeight(e.target.value)}
                  className="w-full border rounded-xl px-3 py-2 outline-none focus:ring-2 focus:ring-yellow-400"
                  placeholder="Enter height"
                />
              </div>
              <button
                type="button"
                onClick={() => {}}
                className="w-full bg-orange-600 hover:bg-orange-700 text-white font-semibold py-2.5 rounded-xl"
              >
                Calculate BMI
              </button>

              <div className="text-gray-600">
                {bmi ? (
                  <span className="font-semibold">Your BMI: {bmi}</span>
                ) : (
                  <span className="text-sm">Enter weight & height to see BMI</span>
                )}
              </div>
            </div>
          </div>
        </div>

        {/* Attendance History */}
        <div className="rounded-2xl border bg-white p-6 shadow-sm mt-6">
          <div className="text-lg font-semibold mb-1">Attendance History</div>
          <div className="text-gray-500 text-sm mb-4">Your gym visit records</div>

          <div className="divide-y">
            {recent.length === 0 && (
              <div className="text-gray-500 text-sm">No attendance yet.</div>
            )}
            {recent.map((x, i) => (
              <div key={i} className="py-4 flex items-center justify-between">
                <div>
                  <div className="font-semibold">{fmtDate(x.checkInTimeUtc)}</div>
                  <div className="text-gray-500 text-sm">
                    Check-in:{" "}
                    {new Date(x.checkInTimeUtc).toLocaleTimeString(undefined, {
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </div>
                </div>
                <div className="text-gray-400 text-sm">{" "}</div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
