import { useEffect, useMemo, useState } from "react";
import {
  getAttendanceRecent,
  getAttendanceSummary,
  getClassesCount,
  getCurrentMembership,
  getWorkoutActiveCount,
} from "../services/api";

function Card({ title, icon, children }) {
  return (
    <div className="rounded-2xl border bg-white p-5 shadow-sm">
      <div className="flex items-center justify-between mb-3">
        <div className="text-sm text-gray-600">{title}</div>
        <div className="text-gray-400">{icon}</div>
      </div>
      {children}
    </div>
  );
}

function formatDate(d) {
  const dt = new Date(d);
  return dt.toLocaleDateString(undefined, {
    month: "numeric",
    day: "numeric",
    year: "numeric",
  });
}
function formatTimeRange(utc) {
  const start = new Date(utc);
  // simple 1.5h placeholder range display like screenshot
  const end = new Date(start.getTime() + 90 * 60 * 1000);
  const fmt = (x) =>
    x.toLocaleTimeString(undefined, { hour: "2-digit", minute: "2-digit" });
  return `${fmt(start)} - ${fmt(end)}`;
}

export default function Dashboard() {
  const [loading, setLoading] = useState(true);
  const [membership, setMembership] = useState(null);
  const [attendance, setAttendance] = useState({ totalVisits: 0, recent: [] });
  const [classesCount, setClassesCount] = useState(0);
  const [workoutsCount, setWorkoutsCount] = useState(0);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const [m, s, c, w, r] = await Promise.allSettled([
          getCurrentMembership(),
          getAttendanceSummary(),
          getClassesCount(),
          getWorkoutActiveCount(),
          getAttendanceRecent(5),
        ]);

        if (!mounted) return;

        if (m.status === "fulfilled") setMembership(m.value.data);
        if (s.status === "fulfilled") setAttendance(s.value.data);
        if (c.status === "fulfilled") setClassesCount(c.value.data.count ?? 0);
        if (w.status === "fulfilled") setWorkoutsCount(w.value.data.count ?? 0);
        if (r.status === "fulfilled")
          setAttendance((prev) => ({ ...prev, recent: r.value.data || [] }));
      } finally {
        if (mounted) setLoading(false);
      }
    })();
    return () => (mounted = false);
  }, []);

  const daysRemaining = useMemo(() => {
    if (!membership?.endDate) return null;
    const diff =
      (new Date(membership.endDate).getTime() - new Date().getTime()) /
      (1000 * 60 * 60 * 24);
    return Math.floor(diff);
  }, [membership]);

  if (loading) {
    return (
      <div className="flex-1 p-8">
        <div className="animate-pulse text-gray-500">Loading dashboard…</div>
      </div>
    );
  }

  const greetingName = (membership?.userFullName || "").split(" ")[0] || "Member";

  return (
    <div className="flex-1 overflow-y-auto">
      <div className="max-w-7xl mx-auto px-8 py-8">
        {/* Header */}
        <h1 className="text-4xl font-extrabold mb-2">Welcome back, {greetingName}!</h1>
        <p className="text-gray-500 mb-6">Here's your fitness overview</p>

        {/* Top grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-5">
          <Card title="Membership Status" icon="🧾">
            <div className="text-2xl font-bold">
              {membership?.status ?? "Inactive"}
            </div>
            <div className="text-gray-500 text-sm">
              {daysRemaining !== null
                ? `${daysRemaining} days remaining`
                : "No active plan"}
            </div>
          </Card>

          <Card title="Attendance" icon="📈">
            <div className="text-2xl font-bold">{attendance?.totalVisits ?? 0}</div>
            <div className="text-gray-500 text-sm">Total visits</div>
          </Card>

          <Card title="Workout Plans" icon="🧱">
            <div className="text-2xl font-bold">{workoutsCount}</div>
            <div className="text-gray-500 text-sm">Active plans</div>
          </Card>

          <Card title="Available Classes" icon="📆">
            <div className="text-2xl font-bold">{classesCount}</div>
            <div className="text-gray-500 text-sm">Classes to book</div>
          </Card>
        </div>

        {/* Bottom grid */}
        <div className="grid grid-cols-1 xl:grid-cols-2 gap-6 mt-6">
          {/* Current Membership */}
          <div className="rounded-2xl border bg-white p-6 shadow-sm">
            <div className="text-lg font-semibold mb-1">Current Membership</div>
            <div className="text-gray-500 text-sm mb-5">
              Your active subscription details
            </div>

            <div className="space-y-4">
              <div>
                <div className="text-gray-600 text-sm">Plan</div>
                <div className="text-xl font-bold">
                  {membership?.planName ?? "—"}
                </div>
              </div>

              <div>
                <div className="text-gray-600 text-sm">Valid Until</div>
                <div className="text-xl font-bold">
                  {membership?.endDate ? formatDate(membership.endDate) : "—"}
                </div>
              </div>

              <div>
                <div className="text-gray-600 text-sm">Status</div>
                <span
                  className={`inline-flex items-center px-2 py-1 rounded-full text-sm font-medium
                    ${
                      (membership?.status ?? "Inactive") === "Active"
                        ? "bg-green-100 text-green-700"
                        : "bg-gray-100 text-gray-600"
                    }`}
                >
                  {membership?.status ?? "Inactive"}
                </span>
              </div>
            </div>
          </div>

          {/* Recent Attendance */}
          <div className="rounded-2xl border bg-white p-6 shadow-sm">
            <div className="text-lg font-semibold mb-1">Recent Attendance</div>
            <div className="text-gray-500 text-sm mb-4">
              Your latest gym visits
            </div>
            <div className="divide-y">
              {(attendance?.recent ?? []).length === 0 && (
                <div className="text-gray-500 text-sm">No visits yet.</div>
              )}
              {(attendance?.recent ?? []).map((x, i) => (
                <div key={i} className="py-4">
                  <div className="font-semibold">
                    {formatDate(x.checkInTimeUtc)}
                  </div>
                  <div className="text-gray-500 text-sm">
                    {formatTimeRange(x.checkInTimeUtc)}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
