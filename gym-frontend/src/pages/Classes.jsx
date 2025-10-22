import React, { useEffect, useState } from "react";
import http from "../api/http";
import ClassCard from "../components/ClassCard";

function toTimeRange(item) {
  // Support either startDateTime/endDateTime or dayOfWeek + startTime
  const start =
    item.startDateTime || item.startTime || item.startsAt || item.start;
  const end = item.endDateTime || item.endTime || item.endsAt || item.end;

  if (start && end) {
    const s = new Date(start);
    const e = new Date(end);
    const sameDay = s.toDateString() === e.toDateString();
    return sameDay
      ? `${s.toLocaleDateString()} • ${s.toLocaleTimeString([], {
          hour: "2-digit",
          minute: "2-digit",
        })} – ${e.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}`
      : `${s.toLocaleString()} – ${e.toLocaleString()}`;
  }

  // fallback: DayOfWeek 18:30
  const dow = item.dayOfWeek || item.weekDay;
  const time = item.startTimeText || item.time;
  if (dow && time) return `${dow} • ${time}`;

  return "Schedule TBA";
}

export default function Classes() {
  const [classes, setClasses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState("");

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        setLoading(true);
        const res = await http.get("/api/Class/all");
        if (!alive) return;
        setClasses(Array.isArray(res.data) ? res.data : []);
      } catch (e) {
        setErr("Failed to load classes.");
      } finally {
        alive && setLoading(false);
      }
    })();
    return () => (alive = false);
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold">Classes</h1>
      <p className="mt-1 text-gray-500">Schedule posted by trainers and admin</p>

      {loading ? (
        <div className="mt-8 text-sm text-gray-500">Loading…</div>
      ) : err ? (
        <div className="mt-8 rounded-lg bg-red-50 p-4 text-red-700">{err}</div>
      ) : classes.length === 0 ? (
        <div className="mt-8 text-sm text-gray-500">No Classes Scheduled Yet</div>
      ) : (
        <div className="mt-6 grid gap-5 md:grid-cols-2">
          {classes.map((c) => {
            const title = c.title || c.name || "Class";
            const trainer =
              c.trainerName || c.instructorName || c.trainer || "—";
            const location = c.location || c.room || c.place;
            const duration =
              c.duration ||
              (c.minutes ? `${c.minutes} min` : c.lengthText) ||
              null;

            return (
              <ClassCard
                key={c.id || title + Math.random()}
                title={title}
                trainer={trainer}
                when={toTimeRange(c)}
                duration={duration}
                location={location}
                onAction={() => {
                  // hook up to details modal / route later
                }}
              />
            );
          })}
        </div>
      )}
    </div>
  );
}
